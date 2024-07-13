using Models.Http;
using Models.Models;
using Models.PostgreSQL;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Models
{
    public class Worker
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        private readonly VacanciesRequesterAndExtractorFromHttpResponse _vacanciesRequesterAndExtractorFromResponse;
        private readonly VacanciesToPostresImporter _vacanciesToPostresImporter;

        public Worker(ILogger<Worker> logger, IConfiguration configuration, VacanciesRequesterAndExtractorFromHttpResponse vacanciesRequesterAndExtractorFromResponse,
            VacanciesToPostresImporter vacanciesToPostresImporter)
        {
            _configuration = configuration;
            _vacanciesRequesterAndExtractorFromResponse = vacanciesRequesterAndExtractorFromResponse;
            _vacanciesToPostresImporter = vacanciesToPostresImporter;
            _logger = logger;
        }


        public async Task ImportVacanciesFromHH_RU_ViaAPI_ToPostgresAsync(string text, string employment = "full", string schedule = "remote", string experience = "noExperience", int? areaId = null, string[] searchFields = null, int page = 0)
        {
            Root root = await _vacanciesRequesterAndExtractorFromResponse.GetVacanciesResponseObjectViaHttpQuery(text, employment, schedule, experience, areaId, searchFields, page);
            if (root is null)
                return;
            if (root.Items is null)
                return;
            if (root.Items.Count == 0)
                return;

            await _vacanciesToPostresImporter.ImportVacanciesFromHH_RU_ViaAPI_ToPostgresAsync(root);

            await ImportVacanciesFromHH_RU_ViaAPI_ToPostgresAsync(text, employment, schedule, experience, areaId, searchFields, ++page);
        }


    }
}

