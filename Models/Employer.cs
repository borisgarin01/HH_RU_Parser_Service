
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace HH_RU_ParserService.Models
{
    public record Employer(
        [property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("id")] string Id,
        [property: JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("name")] string Name,
        [property: JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("url")] string Url,
        [property: JsonProperty("alternate_url", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("alternate_url")] string AlternateUrl,
        [property: JsonProperty("logo_urls", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("logo_urls")] LogoUrls LogoUrls,
        [property: JsonProperty("vacancies_url", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("vacancies_url")] string VacanciesUrl,
        [property: JsonProperty("accredited_it_employer", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("accredited_it_employer")] bool? AccreditedItEmployer,
        [property: JsonProperty("trusted", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("trusted")] bool? Trusted
    );
}