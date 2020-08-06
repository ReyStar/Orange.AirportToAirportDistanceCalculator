using AirportInfo.Domain.Interfaces;
using AirportInfo.Domain.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AirportInfo.Domain
{
    public static class Registration
    {
        public static IServiceCollection RegisterDomain(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IAirportInfoService, AirportInformationService>();
          
            return services;
        }
    }
}
