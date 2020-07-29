using Newtonsoft.Json;

namespace Orange.AirportToAirportDistanceCalculator.Application.Models
{
    class CteleportAirportInfo
    {
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }

        [JsonProperty(PropertyName = "city_iata")]
        public string CityIATA { get; set; }
        
        [JsonProperty(PropertyName = "iata")]
        public string IATA { get; set; }
        
        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }
        
        [JsonProperty(PropertyName = "timezone_region_name")]
        public string TimezoneRegionName { get; set; }
        
        [JsonProperty(PropertyName = "country_iata")]
        public string CountryIATA { get; set; }
        
        [JsonProperty(PropertyName = "rating")]
        public string Rating { get; set; }
        
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        
        [JsonProperty(PropertyName = "location")]
        public CteleportGeoCoordinate Location { get; set; }
        
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        
        [JsonProperty(PropertyName = "hubs")]
        public int Hubs { get; set; }
    }
}
