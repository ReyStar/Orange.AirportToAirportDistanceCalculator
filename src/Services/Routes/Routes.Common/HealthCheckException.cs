using System;

namespace Routes.Common
{
    /// <summary>
    /// This error will be generated if the healthCheck is not passed
    /// </summary>
    public class HealthCheckException : Exception
    {
        public HealthCheckException(string message) 
            : base(message)
        {
        }

        public HealthCheckException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
