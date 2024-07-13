using Dapper;
using HH_RU_ParserService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace HH_RU_ParserService
{

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

        public async Task ImportVacanciesFromHH_RU_ViaAPI_ToPostgresAsync(string text, string employment = "full", string schedule = "remote", string experience = "noExperience", int? areaId = null, string[] searchFields = null, int page = 0)
        {
            HttpResponseMessage responseMessage = null;
            if (searchFields is null)
            {
                if (areaId is null)
                {
                    responseMessage = await httpClient.GetAsync($"https://api.hh.ru/vacancies?text={text}&employment={employment}&schedule={schedule}&page={page}&experience={experience}");
                }
                else
                {
                    responseMessage = await httpClient.GetAsync($"https://api.hh.ru/vacancies?text={text}&employment={employment}&schedule={schedule}&page={page}&experience={experience}&areaId={areaId}");
                }
            }
            else
            {
                if (areaId is null)
                {
                    responseMessage = await httpClient.GetAsync($"https://api.hh.ru/vacancies?text={text}&employment={employment}&schedule={schedule}&page={page}&experience={experience}&{string.Join('&', searchFields.Select(sf => $"search_field={sf}"))}");
                }
                else
                {
                    responseMessage = await httpClient.GetAsync($"https://api.hh.ru/vacancies?text={text}&employment={employment}&schedule={schedule}&page={page}&experience={experience}&{string.Join('&', searchFields.Select(sf => $"search_field={sf}"))}&areaId={areaId}");
                }
            }
            if (responseMessage.IsSuccessStatusCode)
            {
                Root root = await JsonSerializer.DeserializeAsync<Root>(await responseMessage.Content.ReadAsStreamAsync());

                using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    foreach (var item in root.Items)
                    {
                        if (item.Address != null)
                        {
                            await npgsqlConnection.ExecuteAsync(@"
                                INSERT INTO Addresses 
                                    (City,Street,Building,Lat,Lng,Raw,Id) 
                                    VALUES(@City,@Street,@Building,@Lat,@Lng,@Raw,@Id) 
                                ON CONFLICT DO NOTHING;",
                                    new
                                    {
                                        City = !string.IsNullOrWhiteSpace(item.Address.City) ? item.Address.City : "Not set",
                                        Street = !string.IsNullOrWhiteSpace(item.Address.Street) ? item.Address.Street : "Not set",
                                        Building = !string.IsNullOrWhiteSpace(item.Address.Building) ? item.Address.Building : "Not set",
                                        item.Address.Lat,
                                        item.Address.Lng,
                                        Raw = !string.IsNullOrWhiteSpace(item.Address.Raw) ? item.Address.Raw : "Not set",
                                        item.Address.Id
                                    });
                        }
                        if (item.Area != null)
                        {
                            await npgsqlConnection.ExecuteAsync(@"
                                INSERT INTO Areas 
                                (Id, Name, Url) 
                                VALUES(@Id, @Name, @URL) ON CONFLICT DO NOTHING;",
                                new
                                {
                                    item.Area.Id,
                                    item.Area.Name,
                                    item.Area.Url
                                });
                        }

                        if (item.Branding != null)
                        {
                            await npgsqlConnection.ExecuteAsync(@"INSERT INTO Brandings (Id, Type, Tariff) VALUES(@Id, @Type, @Tariff) ON CONFLICT DO NOTHING;", new { item.Id, item.Branding.Type, item.Branding.Tariff });
                        }

                        if (item.Employer != null)
                        {
                            if (item.Employer.LogoUrls != null)
                            {
                                await npgsqlConnection.ExecuteAsync(@"INSERT INTO LogosUrls(_240, _90, Original) 
                                    VALUES(@_240, @_90, @Original) 
                                        ON CONFLICT DO NOTHING;", new
                                {
                                    item.Employer.LogoUrls._240,
                                    item.Employer.LogoUrls._90,
                                    item.Employer.LogoUrls.Original
                                });
                            }

                            await npgsqlConnection.ExecuteAsync(@"INSERT INTO Employers(Id, Name, URL, AlternateURL, 
                                VacanciesURL, AccreditedItEmployer, Trusted, LogosUrlsOriginal) 
                                    VALUES(@Id, @Name, @URL, @Alternate_URL, @Vacancies_URL, @Accredited_It_Employer, 
                                        @Trusted, @LogosUrlsOriginal) 
                                    ON CONFLICT DO NOTHING;", new
                            {

                                item.Employer.Id,
                                item.Employer.Name,
                                item.Employer.Url,
                                Alternate_Url = item.Employer.AlternateUrl,
                                Vacancies_Url = item.Employer.VacanciesUrl,
                                Accredited_It_Employer = item.Employer.AccreditedItEmployer,
                                item.Employer.Trusted,
                                LogosUrlsOriginal = item.Employer.LogoUrls?.Original
                            });
                        }

                        if (item.Employment != null)
                        {
                            await npgsqlConnection.ExecuteAsync("INSERT INTO Employments(Id, Name) VALUES(@Id, @Name) ON CONFLICT DO NOTHING;", new
                            {
                                item.Employment.Id,
                                item.Employment.Name
                            });
                        }

                        if (item.Experience != null)
                        {
                            await npgsqlConnection.ExecuteAsync("INSERT INTO Experiences(Id, Name) VALUES(@Id, @Name) ON CONFLICT DO NOTHING;", new
                            {
                                item.Experience.Id,
                                item.Experience.Name
                            });
                        }

                        if (item.InsiderInterview != null)
                        {
                            await npgsqlConnection.ExecuteAsync("INSERT INTO InsidersInterviews(Id, URL) VALUES(@Id, @URL) ON CONFLICT DO NOTHING;", new
                            {
                                item.InsiderInterview.Id,
                                item.InsiderInterview.Url
                            });
                        }

                        if (item.Address != null)
                        {
                            if (item.Address.Metro != null)
                            {
                                await npgsqlConnection.ExecuteAsync("INSERT INTO Metros(StationName, LineName, StationId, LineId, Lat, Lng) VALUES(@StationName, @LineName, @StationId, @LineId, @Lat, @Lng) ON CONFLICT DO NOTHING;",
                                    new
                                    {
                                        item.Address.Metro.StationName,
                                        item.Address.Metro.LineName,
                                        item.Address.Metro.StationId,
                                        item.Address.Metro.LineId,
                                        item.Address.Metro.Lat,
                                        item.Address.Metro.Lng
                                    });
                            }
                        }

                        if (item.Type != null)
                        {
                            await npgsqlConnection.ExecuteAsync("INSERT INTO Types(Name, Id) VALUES(@Name, @Id) ON CONFLICT DO NOTHING;", new { item.Type.Name, item.Type.Id });
                        }

                        if (item.Schedule != null)
                        {
                            await npgsqlConnection.ExecuteAsync("INSERT INTO Schedules(Id, Name) VALUES(@Id, @Name) ON CONFLICT DO NOTHING;", new
                            {
                                item.Schedule.Id,
                                item.Schedule.Name
                            });
                        }

                        string addressId;
                        if (item.Address != null && !string.IsNullOrWhiteSpace(item.Address.Id))
                            addressId = item.Address.Id;
                        else
                            addressId = null;

                        await npgsqlConnection.ExecuteAsync(@"INSERT INTO Items
                        (
                            Id, Premium, Name, HasTest, ResponseLetterRequired, AreaId, TypeId, AddressId, 
                            PublishedAt, CreatedAt, Archived, ApplyAlternateUrl, ShowLogoInSearch, InsiderInterviewId, Url, 
                            AlternateUrl, EmployerId, ScheduleId, AcceptTemporary, AcceptIncompleteResumes,
                            EmploymentId, IsAdvVacancy, ExperienceId) 
                        VALUES
                        (
                            @Id, @Premium, @Name, @HasTest, @ResponseLetterRequired, @AreaId, @TypeId, @AddressId,
                            @PublishedAt, @CreatedAt, @Archived, @ApplyAlternateUrl, @ShowLogoInSearch, @InsiderInterviewId, @Url,
                            @AlternateUrl, @EmployerId, @ScheduleId, @AcceptTemporary, @AcceptIncompleteResumes, @EmploymentId, @IsAdvVacancy, @ExperienceId) ON CONFLICT DO NOTHING;", new
                        {
                            item.Id,
                            item.Premium,
                            item.Name,
                            item.HasTest,
                            ResponseLetterRequired = item.ResponseLetterRequired.HasValue ? item.ResponseLetterRequired.Value : false,
                            AreaId = item.Area?.Id,
                            TypeId = item.Type.Id,
                            AddressId = addressId,
                            item.PublishedAt,
                            item.CreatedAt,
                            item.Archived,
                            item.ApplyAlternateUrl,
                            ShowLogoInSearch = item.ShowLogoInSearch.HasValue ? item.ShowLogoInSearch.Value : false,
                            InsiderInterviewId = item.InsiderInterview?.Id,
                            item.Url,
                            item.AlternateUrl,
                            EmployerId = item.Employer is not null && !string.IsNullOrWhiteSpace(item.Employer.Id) ? item.Employer.Id : string.Empty,
                            ScheduleId = !string.IsNullOrWhiteSpace(item.Schedule?.Id) ? item.Schedule?.Id : null,
                            item.AcceptTemporary,
                            item.AcceptIncompleteResumes,
                            EmploymentId = item.Employment?.Id,
                            item.IsAdvVacancy,
                            ExperienceId = item.Experience.Id
                        });

                        if (item.Salary != null)
                        {
                            await npgsqlConnection.ExecuteAsync("INSERT INTO Salaries(Id, SalaryFrom, SalaryTo, Currency, Gross) VALUES(@Id, @SalaryFrom, @SalaryTo, @Currency, @Gross) ON CONFLICT DO NOTHING;", new
                            {
                                item.Id,
                                SalaryFrom = item.Salary.From,
                                SalaryTo = item.Salary.To,
                                item.Salary.Currency,
                                item.Salary.Gross
                            });
                        }



                        if (item.Snippet != null)
                        {
                            string requirement = !string.IsNullOrWhiteSpace(item.Snippet.Requirement) ? item.Snippet.Requirement : null;
                            string responsibility = !string.IsNullOrWhiteSpace(item.Snippet.Responsibility) ? item.Snippet.Requirement : null;

                            await npgsqlConnection.ExecuteAsync("INSERT INTO Snippets(Id, Requirement, Responsibility) VALUES(@Id, @Requirement, @Responsibility) ON CONFLICT DO NOTHING;", new { item.Id, requirement, responsibility });
                        }
                    }
                }
                await ImportVacanciesFromHH_RU_ViaAPI_ToPostgresAsync(text, employment, schedule, experience, areaId, searchFields, ++page);
            }
        }
    }
}

