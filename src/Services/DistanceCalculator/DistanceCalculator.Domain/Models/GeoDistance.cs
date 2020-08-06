namespace DistanceCalculator.Domain.Models
{
    /// <summary>
    /// Geo distances
    /// </summary>
    public struct GeoDistance
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GeoDistance"/> struct.
        /// </summary>
        /// <param name="miles">The miles.</param>
        public GeoDistance(double miles)
        {
            Miles = miles;
        }

        /// <summary>
        /// Gets or sets the miles.
        /// </summary>
        public double Miles { get; set; }
    }
}
