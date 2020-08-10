using System;

namespace AccountManager.Domain.Exceptions
{
    /// <summary>
    /// This exception indicates that an error occurred while executing a request
    /// to the service that provides information about the account authentication.
    /// </summary>
    public class AccountAuthenticationException : Exception
    {
        public AccountAuthenticationException(string message) : base(message)
        {
        }

        public AccountAuthenticationException(Exception innerException) 
            : base("Error account authentication", innerException)
        {
        }

        public AccountAuthenticationException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
