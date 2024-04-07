
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace HH_RU_ParserService.Models
{
    public record Area(
        [property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("id")] string? Id,
        [property: JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("name")] string? Name,
        [property: JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("url")] string? Url
    );
}