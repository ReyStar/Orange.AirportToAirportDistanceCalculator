using System;

namespace Routes.Domain.Exceptions
{
    /// <summary>
    /// This exception indicates that an error occurred while executing a request
    /// to the service that provides information about the route.
    /// </summary>
    public class RouteServiceException : Exception
    {
        public RouteServiceException(string message) : base(message)
        {
        }

        public RouteServiceException(Exception innerException) 
            : base("Error getting airport information", innerException)
        {
        }

        public RouteServiceException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
