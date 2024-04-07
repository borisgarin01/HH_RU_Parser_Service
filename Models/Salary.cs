
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace HH_RU_ParserService.Models
{
    public record Salary(
        [property: JsonProperty("from", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("from")] int? From,
        [property: JsonProperty("to", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("to")] int? To,
        [property: JsonProperty("currency", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("currency")] string Currency,
        [property: JsonProperty("gross", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("gross")] bool? Gross
    );
}