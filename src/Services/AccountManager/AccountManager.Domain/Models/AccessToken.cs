using System;

namespace AccountManager.Domain.Models
{
    public class AccessToken
    {
        public Guid AccountId { get; set; }

        public string Token { get; set; }

        public string TokenHash { get; set; }

        public DateTime Expires { get; set; }
        
        public DateTime Created { get; set; }
        
        //BL
        public bool IsExpired => DateTime.UtcNow >= Expires;
    }
}
