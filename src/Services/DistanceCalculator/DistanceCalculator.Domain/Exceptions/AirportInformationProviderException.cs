using System;

namespace DistanceCalculator.Domain.Exceptions
{
    /// <summary>
    /// This exception indicates that an error occurred while executing a request
    /// to the service that provides information about the airport.
    /// </summary>
    public class AirportInformationProviderException : Exception
    {
        public AirportInformationProviderException(string message) : base(message)
        {
        }

        public AirportInformationProviderException(Exception innerException) 
            : base("Error getting airport information", innerException)
        {
        }

        public AirportInformationProviderException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
