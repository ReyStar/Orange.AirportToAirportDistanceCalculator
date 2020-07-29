using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Orange.AirportToAirportDistanceCalculator.Application.Infrastructure;
using Orange.AirportToAirportDistanceCalculator.Application.Interfaces;
using Orange.AirportToAirportDistanceCalculator.Application.Services;
using Orange.AirportToAirportDistanceCalculator.Domain.Interfaces;
using Orange.AirportToAirportDistanceCalculator.Domain.Models;
using Polly;

namespace Orange.AirportToAirportDistanceCalculator.Application
{
    public static class Registration
    {
        public static IServiceCollection RegisterApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<Profile, AutoMapperProfile>();

            services.Configure<CteleportClientConfig>(configuration.GetSection(nameof(CteleportClientConfig)));
            services.AddSingleton<IValidateOptions<CteleportClientConfig>, CteleportClientConfigValidator>();

            var cteleportClientConfig = configuration.GetSection(nameof(CteleportClientConfig)).Get<CteleportClientConfig>();

            // This code a bit smells.
            // need validate manually. we use IConfiguration directly without DI and don't have IOption validation mechanism 
            var validationResult = new CteleportClientConfigValidator().Validate(null, cteleportClientConfig);
            if (validationResult.Failed)
            {
                throw new ArgumentException(validationResult.FailureMessage);
            }

            services.AddTransient<IAirportInformationProviderService, AirportInformationProviderService>();

            services.AddHttpClient(nameof(ICteleportClient),
                    (provider, client) =>
                    {
                        client.BaseAddress = new Uri(cteleportClientConfig.BaseAddress);
                        client.Timeout = cteleportClientConfig.Timeout;
                    })
                    .SetHandlerLifetime(cteleportClientConfig.HandlerLifetime) //if we assume that the dns will change every 1 minute
                    .AddTypedClient(client => Refit.RestService.For<ICteleportClient>(client))
                    .AddHeaderPropagation()
                    .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(cteleportClientConfig.RetryCount, _ => cteleportClientConfig.RetryTimeout))
                    .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(cteleportClientConfig.CircuitBreakerFailTryCount, cteleportClientConfig.CircuitTimeout));

            
            var cacheDataType = configuration.GetValue<CacheDataType>(nameof(CacheDataType), CacheDataType.DataBase);

            // Decorator registration positions make sense
            // A later registered decorator is called earlier.
            if ((CacheDataType.DataBase & cacheDataType) == CacheDataType.DataBase)
            {
                services.Decorate<IDistanceCalculatorService, DistanceCalculatorDataBaseCacheService>();
            }
            if ((CacheDataType.Memory & cacheDataType) == CacheDataType.Memory)
            {
                services.Decorate<IDistanceCalculatorService, DistanceCalculatorMemoryCacheService>();
            }

            return services;
        }
    }
}
