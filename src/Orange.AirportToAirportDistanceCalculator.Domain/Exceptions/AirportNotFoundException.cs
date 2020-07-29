using System;

namespace Orange.AirportToAirportDistanceCalculator.Domain.Exceptions
{
    /// <summary>
    /// This exception indicates that an error occurred when the distance calculation
    /// </summary>
    public class AirportNotFoundException : Exception
    {
        public AirportNotFoundException(string message) 
            : base(message)
        {
        }

        public AirportNotFoundException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
