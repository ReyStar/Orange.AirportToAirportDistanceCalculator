using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Routes.Application.Infrastructure;
using Routes.Application.Services;
using Routes.Domain.Interfaces;

namespace Routes.Application
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
                services.Decorate<IRoutesService, RoutesMemoryCacheService>();
            }

            return services;
        }
    }
}
