using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace HH_RU_ParserService
{
    public sealed class WindowsBackgroundService(
        Worker worker,
        ILogger<WindowsBackgroundService> logger) : BackgroundService
    {
        private int page = 0;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var csharpQueryUrlEncoded = HttpUtility.UrlEncode("C#");
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await worker.ImportVacanciesFromHH_RU_ViaAPI_ToPostgresAsync(csharpQueryUrlEncoded, experience: "between1And3", areaId: 71, schedule: "fullDay");
                    await worker.ImportVacanciesFromHH_RU_ViaAPI_ToPostgresAsync(csharpQueryUrlEncoded, experience: "noExperience", areaId: 71, schedule: "fullDay");

                    await worker.ImportVacanciesFromHH_RU_ViaAPI_ToPostgresAsync(csharpQueryUrlEncoded, experience: "between1And3", areaId: 71);
                    await worker.ImportVacanciesFromHH_RU_ViaAPI_ToPostgresAsync(csharpQueryUrlEncoded, experience: "noExperience", areaId: 71); 
                    
                    await worker.ImportVacanciesFromHH_RU_ViaAPI_ToPostgresAsync(csharpQueryUrlEncoded, experience: "between1And3");
                    await worker.ImportVacanciesFromHH_RU_ViaAPI_ToPostgresAsync(csharpQueryUrlEncoded, experience: "noExperience");

                    logger.LogWarning($"Vacancies imported {DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm")}");

                    await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "{Message}", ex.Message);

                // Terminates this process and returns an exit code to the operating system.
                // This is required to avoid the 'BackgroundServiceExceptionBehavior', which
                // performs one of two scenarios:
                // 1. When set to "Ignore": will do nothing at all, errors cause zombie services.
                // 2. When set to "StopHost": will cleanly stop the host, and log errors.
                //
                // In order for the Windows Service Management system to leverage configured
                // recovery options, we need to terminate the process with a non-zero exit code.
                Environment.Exit(1);
            }
        }
    }
}

