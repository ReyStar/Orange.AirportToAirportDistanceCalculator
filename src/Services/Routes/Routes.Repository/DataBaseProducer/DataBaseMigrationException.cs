#nullable enable
using System;

namespace Routes.Repository.DataBaseProducer
{
    /// <summary>
    /// Exception thrown in case of failed migration
    /// </summary>
    public class DataBaseMigrationException : Exception
    {
        public DataBaseMigrationException(string? message) 
            : base(message)
        {
        }

        public DataBaseMigrationException(string? message, Exception? innerException) 
            : base(message, innerException)
        {
        }
    }
}
