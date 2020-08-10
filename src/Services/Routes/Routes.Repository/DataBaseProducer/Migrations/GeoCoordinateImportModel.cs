namespace Routes.Repository.DataBaseProducer.Migrations
{
    struct GeoCoordinateImportModel
    {
        public GeoCoordinateImportModel(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        public double Longitude { get; set; }
    }
}
