using System;

namespace Routes.Shell.Configuration
{
    public class AuthenticationConfig
    {
        public string SecretKey { get; set; }

        public TimeSpan ClockSkew { get; set; } = TimeSpan.FromSeconds(5);
    }
}
