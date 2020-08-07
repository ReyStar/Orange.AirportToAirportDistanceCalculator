using Newtonsoft.Json;

namespace DistanceCalculator.Application.Models
{
    class AirportInfo
    {
        public string Continent { get; set; }

        public GeoCoordinate Location { get; set; }

        public string ElevationFt { get; set; }

        public string GpsCode { get; set; }

        public string IATACode { get; set; }

        public string Ident { get; set; }

        public string IsoCountry { get; set; }

        public string IsoRegion { get; set; }

        public string LocalCode { get; set; }

        public string Municipality { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }
    }
}
