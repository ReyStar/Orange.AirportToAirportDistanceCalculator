using System;

namespace AccountManager.Repository.Models
{
    public class AccessTokenDbModel
    {
        public Guid AccountId { get; set; }

        public string Token { get; set; }

        public string TokenHash { get; set; }

        public DateTime Expires { get; set; }
        
        public DateTime Created { get; set; }

        public bool IsDeleted { get; set; }
    }
}
