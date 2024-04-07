
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace HH_RU_ParserService.Models
{
    public record Snippet(
        [property: JsonProperty("requirement", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("requirement")] string Requirement,
        [property: JsonProperty("responsibility", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("responsibility")] string Responsibility
    );
}