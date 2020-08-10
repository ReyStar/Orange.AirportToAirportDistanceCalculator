using System;

namespace AccountManager.Repository.Models
{
    /// <summary>
    /// Db model for Save and load user account
    /// </summary>
    class AccountDbModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }

        public int Revision { get; set; }

        public bool IsDeleted { get; set; }
    }
}
