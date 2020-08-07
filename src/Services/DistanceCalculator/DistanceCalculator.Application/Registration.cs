using System;
using System.Net.Http;
using AutoMapper;
using DistanceCalculator.Application.Infrastructure;
using DistanceCalculator.Application.Interfaces;
using DistanceCalculator.Application.Services;
using DistanceCalculator.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;

namespace DistanceCalculator.Application
{
    public static class Registration
    {
        public static IServiceCollection RegisterApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<Profile, AutoMapperProfile>();

            services.Configure<AirportInfoClientConfig>(configuration.GetSection(nameof(AirportInfoClientConfig)));
            services.AddSingleton<IValidateOptions<AirportInfoClientConfig>, AirportInfoClientConfigValidator>();

            var cteleportClientConfig = configuration.GetSection(nameof(AirportInfoClientConfig)).Get<AirportInfoClientConfig>();

            // This code a bit smells.
            // need validate manually. we use IConfiguration directly without DI and don't have IOption validation mechanism 
            var validationResult = new AirportInfoClientConfigValidator().Validate(null, cteleportClientConfig);
            if (validationResult.Failed)
            {
                throw new ArgumentException(validationResult.FailureMessage);
            }

            services.AddTransient<IAirportInformationProviderService, AirportInformationProviderService>();

            var httpClientBuilder =
            services.AddHttpClient(nameof(IAirportInfoClient),
                    (provider, client) =>
                    {
                        client.BaseAddress = new Uri(cteleportClientConfig.BaseAddress);
                        client.Timeout = cteleportClientConfig.Timeout;
                    })
                    .SetHandlerLifetime(cteleportClientConfig.HandlerLifetime) //if we assume that the dns will change every 1 minute
                    .AddTypedClient(client => Refit.RestService.For<IAirportInfoClient>(client))
                    .AddHeaderPropagation()
                    .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(cteleportClientConfig.RetryCount, _ => cteleportClientConfig.RetryTimeout))
                    .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(cteleportClientConfig.CircuitBreakerFailTryCount, cteleportClientConfig.CircuitTimeout));

            //TODO REMOVE this Development crutch; Fix certificate errors 
            if (string.Equals(configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT"), "Development", StringComparison.OrdinalIgnoreCase))
            {
                httpClientBuilder
                    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                    {
                        ClientCertificateOptions = ClientCertificateOption.Manual,
                        ServerCertificateCustomValidationCallback =
                            (httpRequestMessage, cert, cetChain, policyErrors) => { return true; }
                    });
            }

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
