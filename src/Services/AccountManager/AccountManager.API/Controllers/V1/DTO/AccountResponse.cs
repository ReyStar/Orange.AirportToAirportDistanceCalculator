using System;

namespace AccountManager.API.Controllers.V1.DTO
{
    public class AccountResponse
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
