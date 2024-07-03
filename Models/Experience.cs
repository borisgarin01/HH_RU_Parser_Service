
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace HH_RU_ParserService.Models;

public sealed record Experience(
    [property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("id")] string Id,
    [property: JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
    [property: JsonPropertyName("name")] string Name
);