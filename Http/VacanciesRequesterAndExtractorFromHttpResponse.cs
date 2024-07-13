using Models.Models;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Models.Http
{
    public sealed class VacanciesRequesterAndExtractorFromHttpResponse
    {
        HttpClient httpClient;
        public VacanciesRequesterAndExtractorFromHttpResponse()
        {
            httpClient = new();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        }
        public async Task<Root> GetVacanciesResponseObjectViaHttpQuery(string text, string employment = "full", string schedule = "remote", string experience = "noExperience", int? areaId = null, string[] searchFields = null, int page = 0)
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
            Root root = null;
            if (responseMessage.IsSuccessStatusCode)
            {
                root = await JsonSerializer.DeserializeAsync<Root>(await responseMessage.Content.ReadAsStreamAsync());
            }

            return root;
        }
    }
}
