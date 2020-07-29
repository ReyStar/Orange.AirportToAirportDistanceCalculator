namespace Orange.AirportToAirportDistanceCalculator.Domain.Models
{
    /// <summary>
    /// Airport information
    /// </summary>
    public struct AirportInformation
    {
        /// <summary>Initializes a new instance of the <see cref="AirportInformation" /> struct.</summary>
        /// <param name="IATA">The iata airport.</param>
        /// <param name="location">The geo coordinate.</param>
        public AirportInformation(string IATA, GeoCoordinate location)
        {
            this.IATA = IATA;
            Location = location;
        }

        /// <summary>
        /// Gets or sets the airport IATA.
        /// </summary>
        public string IATA { get; set; }

        /// <summary>
        /// Gets or sets the geo location.
        /// </summary>
        public GeoCoordinate Location { get; set; }  
    }
}
