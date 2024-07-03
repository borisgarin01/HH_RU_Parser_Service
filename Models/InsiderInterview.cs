
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace HH_RU_ParserService.Models;

public sealed record InsiderInterview(
    [property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("id")] string Id,
    [property: JsonProperty("url", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("url")] string Url
);