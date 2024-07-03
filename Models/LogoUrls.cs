
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace HH_RU_ParserService.Models;

public sealed record LogoUrls(
    [property: JsonProperty("240", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("240")] string _240,
    [property: JsonProperty("90", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("90")] string _90,
    [property: JsonProperty("original", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("original")] string Original
);