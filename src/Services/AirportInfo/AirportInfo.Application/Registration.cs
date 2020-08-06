using AirportInfo.Application.Infrastructure;
using AirportInfo.Application.Services;
using AirportInfo.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AirportInfo.Application
{
    public static class Registration
    {
        public static IServiceCollection RegisterApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<Profile, AutoMapperProfile>();
            
            var cacheDataType = configuration.GetValue<CacheDataType>(nameof(CacheDataType), CacheDataType.Memory);

            // Decorator registration positions make sense
            // A later registered decorator is called earlier.
            if ((CacheDataType.Memory & cacheDataType) == CacheDataType.Memory)
            {
                services.Decorate<IAirportInfoService, AirportInformationMemoryCacheService>();
            }

            return services;
        }
    }
}
