using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Routes.Domain.Interfaces;
using Routes.Domain.Services;

namespace Routes.Domain
{
    public static class Registration
    {
        public static IServiceCollection RegisterDomain(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IRoutesService, RoutesService>();
          
            return services;
        }
    }
}
