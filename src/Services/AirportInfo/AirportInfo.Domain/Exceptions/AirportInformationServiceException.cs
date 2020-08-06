using System;

namespace AirportInfo.Domain.Exceptions
{
    /// <summary>
    /// This exception indicates that an error occurred while executing a request
    /// to the service that provides information about the airport.
    /// </summary>
    public class AirportInformationServiceException : Exception
    {
        public AirportInformationServiceException(string message) : base(message)
        {
        }

        public AirportInformationServiceException(Exception innerException) 
            : base("Error getting airport information", innerException)
        {
        }

        public AirportInformationServiceException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
