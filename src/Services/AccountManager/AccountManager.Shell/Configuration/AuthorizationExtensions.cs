using System.Text;
using AccountManager.Domain.Infrastructure;
using AccountManager.Shell.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AccountManager.Shell.Configuration
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddOrangeAuthorization(this IServiceCollection services,
                                                                IConfiguration configuration)
        {
            var accountManagerConfig = configuration.GetSection(nameof(AccountManagerConfig)).Get<AccountManagerConfig>();
            var key = Encoding.ASCII.GetBytes(accountManagerConfig.SecretKey);
            var issuerSigningKey = new SymmetricSecurityKey(key);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddScheme<JwtBearerOptions, AuthenticationJwtBearerHandler>(JwtBearerDefaults.AuthenticationScheme, o =>
            {
                o.TokenValidationParameters.IssuerSigningKey = issuerSigningKey;
                o.TokenValidationParameters.ValidateIssuer = false;
                o.TokenValidationParameters.ValidateAudience = false;
                o.TokenValidationParameters.ClockSkew = accountManagerConfig.ClockSkew;
                o.TokenValidationParameters.ValidateIssuerSigningKey = false;

                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
            });


            return services;
        }
    }
}
