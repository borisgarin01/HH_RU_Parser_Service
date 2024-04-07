
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HH_RU_ParserService.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    [Table("Addresses")]
    public record Address(
        [property: JsonProperty("city", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("city")] string City,
        [property: JsonProperty("street", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("street")] string Street,
        [property: JsonProperty("building", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("building")] string Building,
        [property: JsonProperty("lat", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("lat")] double? Lat,
        [property: JsonProperty("lng", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("lng")] double? Lng,
        [property: JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("description")] object Description,
        [property: JsonProperty("raw", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("raw")] string Raw,
        [property: JsonProperty("metro", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("metro")] Metro Metro,
        [property: JsonProperty("metro_stations", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("metro_stations")] IReadOnlyList<MetroStation> MetroStations,
        [property: JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("id")] string Id
    );
}