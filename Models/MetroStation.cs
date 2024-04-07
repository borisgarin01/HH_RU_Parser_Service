
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace HH_RU_ParserService.Models
{
    public record MetroStation(
        [property: JsonProperty("station_name", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("station_name")] string StationName,
        [property: JsonProperty("line_name", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("line_name")] string LineName,
        [property: JsonProperty("station_id", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("station_id")] string StationId,
        [property: JsonProperty("line_id", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("line_id")] string LineId,
        [property: JsonProperty("lat", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("lat")] double? Lat,
        [property: JsonProperty("lng", NullValueHandling = NullValueHandling.Ignore)]
        [property: JsonPropertyName("lng")] double? Lng
    );
}