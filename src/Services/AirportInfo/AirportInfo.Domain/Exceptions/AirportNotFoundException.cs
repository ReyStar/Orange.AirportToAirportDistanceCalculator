using System;

namespace AirportInfo.Domain.Exceptions
{
    /// <summary>
    /// This exception indicates that an error occurred when airport not found
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
