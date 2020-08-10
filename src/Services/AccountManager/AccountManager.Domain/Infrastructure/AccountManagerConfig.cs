using System;

namespace AccountManager.Domain.Infrastructure
{
    public class AccountManagerConfig
    {
        public string SecretKey { get; set; }

        public string Algorithm { get; set; } = "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256";

        public TimeSpan AccessTokenExpires { get; set; } = TimeSpan.FromMinutes(15);
        
        public TimeSpan RefreshTokenExpires { get; set; } = TimeSpan.FromDays(7);

        public TimeSpan ClockSkew { get; set; } = TimeSpan.FromSeconds(5);
    }
}
