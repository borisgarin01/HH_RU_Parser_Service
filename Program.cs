using FluentMigrator.Runner;
using HH_RU_ParserService.Http;
using HH_RU_ParserService.Migrations;
using HH_RU_ParserService.PostgreSQL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;

namespace HH_RU_ParserService
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.Services.AddSingleton<VacanciesRequesterAndExtractorFromHttpResponse>();
            builder.Services.AddSingleton<VacanciesToPostresImporter>();

            builder.Services.AddWindowsService(options => options.ServiceName = "HH RU Parser Service");
            builder.Configuration.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)).AddJsonFile("appsettings.json");
            builder.Services.AddSingleton<Worker>();
            builder.Services.AddHostedService<WindowsBackgroundService>();
            var serviceProvider = new ServiceCollection()
     // Logging is the replacement for the old IAnnouncer
     .AddLogging(lb => lb.AddFluentMigratorConsole())
     // Registration of all FluentMigrator-specific services
     .AddFluentMigratorCore()
     // Configure the runner
     .ConfigureRunner(
         b => b
             // Use SQLite
             .AddPostgres()
             .ConfigureGlobalProcessorOptions(opt =>
             {
                 opt.ProviderSwitches = "Force Quote=false";
             })
             // The SQLite connection string
             .WithGlobalConnectionString(options => builder.Configuration.GetConnectionString("DefaultConnection"))
             // Specify the assembly with the migrations
             .WithMigrationsIn(typeof(InitialMigration).Assembly))
     .BuildServiceProvider();

            // Put the database update into a scope to ensure
            // that all resources will be disposed.
            using (var scope = serviceProvider.CreateScope())
            {
                // Instantiate the runner
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                runner.ListMigrations();
                // Execute the migrations
                runner.MigrateUp();
            }
            var host = builder.Build();
            host.Run();
        }
    }
}