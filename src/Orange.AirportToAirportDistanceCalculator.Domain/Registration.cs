using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orange.AirportToAirportDistanceCalculator.Domain.Interfaces;
using Orange.AirportToAirportDistanceCalculator.Domain.Services;

namespace Orange.AirportToAirportDistanceCalculator.Domain
{
    public static class Registration
    {
        public static IServiceCollection RegisterDomain(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IDistanceCalculatorService, DistanceCalculatorService>();

            var algorithm = configuration.GetValue<GeoCalculationAlgorithmType>(nameof(GeoCalculationAlgorithmType),
                                                                                GeoCalculationAlgorithmType.Haversine);
            switch (algorithm)
            {
                case GeoCalculationAlgorithmType.Vincenty:
                {
                    services.AddSingleton<IGeoCalculatorService, VincentGeoCalculatorService>();
                    break;
                }
                case GeoCalculationAlgorithmType.SphericalCosinesLaw:
                {
                    services.AddSingleton<IGeoCalculatorService, SphericalCosinesLawGeoCalculatorService>();
                    break;
                }
                case GeoCalculationAlgorithmType.Haversine:
                default:
                {
                    services.AddSingleton<IGeoCalculatorService, HaversineGeoCalculatorService>();
                    break;
                }
            }

            return services;
        }
    }
}
