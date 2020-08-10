namespace Routes.Repository.Models
{
    /// <summary>
    /// Geo coordinate for Db storage
    /// </summary>
    class GeoCoordinateDbModel
    {
        ///// <summary>
        ///// Initializes a new instance of the <see cref="GeoCoordinateDbModel"/> struct.
        ///// </summary>
        public GeoCoordinateDbModel()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeoCoordinateDbModel"/> struct.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longitude">The longitude.</param>
        public GeoCoordinateDbModel(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        /// <summary>
        /// Gets or sets the latitude.
        /// </summary>
        /// <value>
        /// The latitude.
        /// </value>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude.
        /// </summary>
        /// <value>
        /// The longitude.
        /// </value>
        public double Longitude { get; set; }
    }
}
