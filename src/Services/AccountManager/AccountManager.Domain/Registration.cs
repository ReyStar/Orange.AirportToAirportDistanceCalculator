using AccountManager.Domain.Infrastructure;
using AccountManager.Domain.Interfaces;
using AccountManager.Domain.Services;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AccountManager.Domain
{
    public static class Registration
    {
        public static IServiceCollection RegisterDomain(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<Profile, AutoMapperProfile>();

            services.AddSingleton<IAccountManagerService, AccountManagerService>();

            services.Configure<AccountManagerConfig>(configuration.GetSection(nameof(AccountManagerConfig)));

            services.AddSingleton<IValidateOptions<AccountManagerConfig>, AccountManagerConfigValidator>();

            services.AddSingleton<IAccountAuthenticationService, AccountAuthenticationService>();

            return services;
        }
    }
}
