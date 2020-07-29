namespace Orange.AirportToAirportDistanceCalculator.Repository.Models
{
    /// <summary>
    /// Db model for Save and load calculated distance between two IATA point
    /// </summary>
    class GeoDistanceDbModel
    {
        public string DepartureIATACode { get; set; }
        
        public string DestinationIATACode { get; set; }

        public double Distance { get; set; }
    }
}
