using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HH_RU_ParserService.Models
{

    public record Root(
        [property: JsonProperty("items", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("items")] IReadOnlyList<Item> Items,
        [property: JsonProperty("found", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("found")] int? Found,
        [property: JsonProperty("pages", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("pages")] int? Pages,
        [property: JsonProperty("page", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("page")] int? Page,
        [property: JsonProperty("per_page", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("per_page")] int? PerPage,
        [property: JsonProperty("clusters", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("clusters")] object Clusters,
        [property: JsonProperty("arguments", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("arguments")] object Arguments,
        [property: JsonProperty("fixes", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("fixes")] object Fixes,
        [property: JsonProperty("suggests", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("suggests")] object Suggests,
        [property: JsonProperty("alternate_url", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("alternate_url")] string AlternateUrl
    );
}