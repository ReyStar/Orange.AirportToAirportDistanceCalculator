using System;

namespace Routes.Domain.Exceptions
{
    /// <summary>
    /// This exception indicates that an error occurred when route not found
    /// </summary>
    public class RouteNotFoundException : Exception
    {
        public RouteNotFoundException(string message) 
            : base(message)
        {
        }

        public RouteNotFoundException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
