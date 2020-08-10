using System;

namespace AccountManager.Domain.Exceptions
{
    /// <summary>
    /// This exception indicates that an error occurred while executing a request
    /// to the service that provides information about the accounts.
    /// </summary>
    public class AccountManagerException : Exception
    {
        public AccountManagerException(string message) : base(message)
        {
        }

        public AccountManagerException(Exception innerException) 
            : base("Error getting account information", innerException)
        {
        }

        public AccountManagerException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
