using Microsoft.Extensions.Caching.Memory;
using Orange.AirportToAirportDistanceCalculator.Domain.Interfaces;
using Orange.AirportToAirportDistanceCalculator.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Orange.AirportToAirportDistanceCalculator.Application.Services
{
    /// <summary>
    /// In memory caching decorator for the DistanceCalculatorService
    /// </summary>
    class DistanceCalculatorMemoryCacheService : IDistanceCalculatorService
    {
        private readonly IDistanceCalculatorService _calculatorService;
        private readonly IMemoryCache _memoryCache;

        public DistanceCalculatorMemoryCacheService(IDistanceCalculatorService calculatorService, IMemoryCache memoryCache)
        {
            _calculatorService = calculatorService;
            _memoryCache = memoryCache;
        }

        public async Task<GeoDistance> CalculateDistanceAsync(string departureIATACode, string destinationIATACode, CancellationToken cancellationToken = default)
        {
            var naturalId = GenerateNaturalId(departureIATACode, destinationIATACode);

            if(_memoryCache.TryGetValue<GeoDistance>(naturalId, out var geoDistance))
            {
                return geoDistance;
            }

            geoDistance = await _calculatorService.CalculateDistanceAsync(departureIATACode, destinationIATACode, cancellationToken);

            _memoryCache.Set(naturalId, geoDistance);

            return geoDistance;
        }

        private string GenerateNaturalId(string departureIATACode, string destinationIATACode)
        {
            if (string.Compare(departureIATACode, destinationIATACode, StringComparison.OrdinalIgnoreCase) > 0)
            {
                return $"{destinationIATACode}-{departureIATACode}";
            }

            return $"{departureIATACode}-{destinationIATACode}";
        }
    }
}
