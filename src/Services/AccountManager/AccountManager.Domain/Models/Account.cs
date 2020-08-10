using System;

namespace AccountManager.Domain.Models
{
    public class Account
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }
    }
}
