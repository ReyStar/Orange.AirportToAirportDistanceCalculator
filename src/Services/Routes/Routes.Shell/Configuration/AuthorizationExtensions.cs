using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Routes.Shell.Configuration
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddOrangeAuthorization(this IServiceCollection services,
                                                                IConfiguration configuration)
        {
            var accountManagerConfig = configuration.GetSection(nameof(AuthenticationConfig)).Get<AuthenticationConfig>();
            var key = Encoding.ASCII.GetBytes(accountManagerConfig.SecretKey);
            var issuerSigningKey = new SymmetricSecurityKey(key);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
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
