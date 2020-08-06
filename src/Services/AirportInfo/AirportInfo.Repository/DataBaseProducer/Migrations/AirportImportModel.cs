using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace AirportInfo.Repository.DataBaseProducer.Migrations
{
    class AirportImportModel
    {
        [JsonProperty("continent")]
        public string Continent { get; set; }

        [BsonIgnore]
        [JsonProperty("coordinates")]
        public string Coordinates { get; set; }

        [JsonProperty("elevation_ft")]
        public string ElevationFt { get; set; }

        [JsonProperty("gps_code")]
        public string GpsCode { get; set; }

        [JsonProperty("iata_code")]
        public string IATACode { get; set; }

        [JsonProperty("ident")]
        public string Ident { get; set; }

        [JsonProperty("iso_country")]
        public string IsoCountry { get; set; }

        [JsonProperty("iso_region")]
        public string IsoRegion { get; set; }

        [JsonProperty("local_code")]
        public string LocalCode { get; set; }

        [JsonProperty("municipality")]
        public string Municipality { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        public GeoCoordinateImportModel Location
        {
            get
            {
                var stringCoordinates = Coordinates.Split(',');

                return new GeoCoordinateImportModel(double.Parse(stringCoordinates[0]),
                    double.Parse(stringCoordinates[1]));

            }
        }
    }
}
