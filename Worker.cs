using Dapper;
using HH_RU_ParserService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace HH_RU_ParserService
{
    public sealed class WindowsBackgroundService(
        Worker worker,
        ILogger<WindowsBackgroundService> logger) : BackgroundService
    {
        private int page = 0;
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    await worker.ImportVacanciesFromHH_RU_ViaPI_ToPostgresAsync(page);
                    logger.LogWarning($"Vacancies imported {DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm")}");

                    await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
                }
            }
            catch (OperationCanceledException)
            {
                // When the stopping token is canceled, for example, a call made from services.msc,
                // we shouldn't exit with a non-zero exit code. In other words, this is expected...
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

    public class Worker
    {
        HttpClient httpClient = new HttpClient();
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
            _configuration = configuration;
            _logger = logger;
        }

        public async Task ImportVacanciesFromHH_RU_ViaPI_ToPostgresAsync(int page)
        {
            var httpResponseMessage = await httpClient.GetAsync($"https://api.hh.ru/vacancies?text=C%23&search_field=name&employment=full&schedule=remote&page={page}");

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                Root root = await JsonSerializer.DeserializeAsync<Root>(await httpResponseMessage.Content.ReadAsStreamAsync());

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    foreach (var item in root.Items)
                    {
                        if (item.Address != null)
                        {
                            await npgsqlConnection.ExecuteAsync("INSERT INTO \"Addresses\" (\"City\",\"Street\",\"Building\",\"Lat\",\"Lng\",\"Raw\",\"Id\") VALUES(@City,@Street,@Building,@Lat,@Lng,@Raw,@Id);", new { item.Address.City, item.Address.Street, item.Address.Building, item.Address.Lat, item.Address.Lng, item.Address.Raw, item.Address.Id });
                        }
                        if (item.Area != null)
                        {
                            await npgsqlConnection.ExecuteAsync("INSERT INTO \"Areas\" (\"Id\", \"Name\", \"Url\") VALUES(@Id, @Name, @URL);", new { item.Area.Id, item.Area.Name, item.Area.Url });
                        }
                        if (item.Branding != null)
                        {
                            await npgsqlConnection.ExecuteAsync("INSERT INTO \"Brandings\" (\"Type\", \"Tariff\") VALUES(@Type, @Tariff);", new { item.Branding.Type, item.Branding.Tariff });
                        }

                        if (item.Employer != null)
                        {
                            await npgsqlConnection.ExecuteAsync("INSERT INTO \"Employers\"(\"Id\", \"Name\", \"URL\", \"Alternate_URL\", \"Vacancies_URL\", \"Accredited_It_Employer\", \"Trusted\") VALUES(@Id, @Name, @URL, @Alternate_URL, @Vacancies_URL, @Accredited_It_Employer, @Trusted);", new { item.Employer.Id, item.Employer.Name, item.Employer.Url, Alternate_Url = item.Employer.AlternateUrl, Vacancies_Url = item.Employer.VacanciesUrl, Accredited_It_Employer = item.Employer.AccreditedItEmployer, item.Employer.Trusted });
                        }

                        if (item.Employment != null)
                        {
                            await npgsqlConnection.ExecuteAsync("INSERT INTO \"Employments\"(\"Id\", \"Name\") VALUES(@Id, @Name);", new { item.Employment.Id, item.Employment.Name });
                        }

                        if (item.Experience != null)
                        {
                            await npgsqlConnection.ExecuteAsync("INSERT INTO \"Experiences\"(\"Id\", \"Name\") VALUES(@Id, @Name);", new { item.Experience.Id, item.Experience.Name });
                        }

                        if (item.InsiderInterview != null)
                        {
                            await npgsqlConnection.ExecuteAsync("INSERT INTO \"InsidersInterviews\"(\"Id\", \"URL\") VALUES(@Id, @URL);", new { item.InsiderInterview.Id, item.InsiderInterview.Url });
                        }

                        await npgsqlConnection.ExecuteAsync("INSERT INTO \"Items\"(\"Id\", \"Premium\", \"Name\", \"Has_Test\", \"Response_Letter_Required\", \"Created_At\", \"Archived\", \"Apply_Alternate_URL\", \"Show_Logo_In_Search\", \"URL\", \"Alternate_URL\", \"Accept_Temporary\", \"Accept_Incomplete_Resumes\", \"Is_Adv_Vacancy\") VALUES(@Id, @Premium, @Name, @Has_Test, @Response_Letter_Required, @Created_At, @Archived, @Apply_Alternate_URL, @Show_Logo_In_Search, @URL, @Alternate_URL, @Accept_Temporary, @Accept_Incomplete_Resumes, @Is_Adv_Vacancy);", new { item.Id, item.Premium, item.Name, Has_Test = item.HasTest, Response_Letter_Required = item.ResponseLetterRequired, Created_At = item.CreatedAt, item.Archived, Apply_Alternate_URL = item.ApplyAlternateUrl, Show_Logo_In_Search = item.ShowLogoInSearch, item.Url, Alternate_URL = item.AlternateUrl, Accept_Temporary = item.AcceptTemporary, Accept_Incomplete_Resumes = item.AcceptIncompleteResumes, Is_Adv_Vacancy = item.IsAdvVacancy });

                        if (item.Employer != null)
                        {
                            if (item.Employer.LogoUrls != null)
                            {
                                await npgsqlConnection.ExecuteAsync("INSERT INTO \"Logo_URLs\"(\"_240\", \"_90\", \"Original\") VALUES(@_240, @_90, @Original)", new { item.Employer.LogoUrls._240, item.Employer.LogoUrls._90, item.Employer.LogoUrls.Original });
                            }
                        }
                        if (item.Address != null)
                        {
                            if (item.Address.Metro != null)
                            {
                                await npgsqlConnection.ExecuteAsync("INSERT INTO \"Metros\"(\"Station_Name\", \"Line_Name\", \"Station_Id\", \"Line_Id\", \"Lat\", \"Lng\") VALUES(@Station_Name, @Line_Name, @Station_Id, @Line_Id, @Lat, @Lng);", new { Station_Name = item.Address.Metro.StationName, Line_Name = item.Address.Metro.LineName, Station_Id = item.Address.Metro.StationId, Line_Id = item.Address.Metro.LineId, item.Address.Metro.Lat, item.Address.Metro.Lng });
                            }
                        }

                        if (item.Salary != null)
                        {
                            await npgsqlConnection.ExecuteAsync("INSERT INTO \"Salaries\"(\"From\", \"To\", \"Currency\", \"Gross\") VALUES(@From, @To, @Currency, @Gross)", new { item.Salary.From, item.Salary.To, item.Salary.Currency, item.Salary.Gross });
                        }

                        if (item.Schedule != null)
                        {
                            await npgsqlConnection.ExecuteAsync("INSERT INTO \"Schedules\"(\"Id\", \"Name\") VALUES(@Id, @Name);", new { item.Schedule.Id, item.Schedule.Name });
                        }

                        if (item.Snippet != null)
                        {
                            await npgsqlConnection.ExecuteAsync("INSERT INTO \"Snippets\"(\"Requirement\", \"Responsibility\") VALUES(@Requirement, @Responsibility);", new { item.Snippet.Requirement, item.Snippet.Responsibility });
                        }

                        if (item.Type != null)
                        {
                            await npgsqlConnection.ExecuteAsync("INSERT INTO \"Types\"(\"Name\") VALUES(@Name);", new { item.Type.Name });
                        }
                    }

                    await npgsqlConnection.ExecuteAsync("INSERT INTO \"Roots\"(\"Found\", \"Pages\", \"Per_Page\", \"Alternate_URL\") VALUES(@Found, @Pages, @Per_Page, @Alternate_URL);", new { root.Found, root.Pages, Per_Page = root.PerPage, Alternate_URL = root.AlternateUrl });
                }
                await ImportVacanciesFromHH_RU_ViaPI_ToPostgresAsync(++page);
            }
        }
    }
}

