using Newtonsoft.Json;

namespace Orange.AirportToAirportDistanceCalculator.Application.Models
{
    class CteleportGeoCoordinate
    {
        [JsonProperty(PropertyName = "lon")]
        public double Longitude { get; set; }
        
        [JsonProperty(PropertyName = "lat")]
        public double Latitude { get; set; }
    }
}
