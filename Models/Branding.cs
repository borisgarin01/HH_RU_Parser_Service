
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace HH_RU_ParserService.Models;

public sealed record Branding(
    [property: JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("type")] string Type,
    [property: JsonProperty("tariff", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("tariff")] string Tariff
);