namespace AirportInfo.Domain.Models
{
    /// <summary>
    /// Airport information
    /// </summary>
    public class AirportInformation
    {
        /// <summary>
        /// <summary>Initializes a new instance of the <see cref="AirportInformation" /> struct.</summary>
        /// </summary>
        public AirportInformation()
        {
        }

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
