namespace AirportInfo.API.Controllers.V1.DTO
{
    public struct AirportInfoResponse
    {
        public string Continent { get; set; }

        public GeoCoordinateResponse Location { get; set; }

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
