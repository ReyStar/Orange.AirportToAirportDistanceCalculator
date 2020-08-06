using System;
using System.Threading;
using System.Threading.Tasks;
using DistanceCalculator.Application.Interfaces;
using DistanceCalculator.Common;
using DistanceCalculator.Domain.Interfaces;
using DistanceCalculator.Domain.Models;
using Microsoft.Extensions.Logging;

namespace DistanceCalculator.Application.Services
{
    /// <summary>
    /// DataBase caching decorator for the DistanceCalculatorService
    /// </summary>
    class DistanceCalculatorDataBaseCacheService : IDistanceCalculatorService
    {
        private readonly IDistanceCalculatorService _calculatorService;
        private readonly IDistancesRepository _distancesRepository;
        private readonly IBackgroundTaskProvider _backgroundTaskProvider;
        private readonly ILogger _logger;

        public DistanceCalculatorDataBaseCacheService(IDistanceCalculatorService calculatorService, 
                                              IDistancesRepository distancesRepository,
                                              IBackgroundTaskProvider backgroundTaskProvider,
                                              ILogger<DistanceCalculatorDataBaseCacheService> logger)
        {
            _calculatorService = calculatorService;
            _distancesRepository = distancesRepository;
            _backgroundTaskProvider = backgroundTaskProvider;
            _logger = logger;
        }

        public async Task<GeoDistance> CalculateDistanceAsync(string departureIATACode, string destinationIATACode, CancellationToken cancellationToken)
        {
            // Special case if the departure airport is equal to the destination airport
            // in this place don't IATA code exists
            if (string.Equals(departureIATACode, destinationIATACode,
                StringComparison.OrdinalIgnoreCase))
            {
                return new GeoDistance(0.0);
            }

            double? savedGeoDistance = null;
            try
            {
                savedGeoDistance = await _distancesRepository.GetDistanceAsync(departureIATACode, destinationIATACode, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error requesting data from the database: {@ex}", ex);
            }

            if (savedGeoDistance != null)
            {
                return new GeoDistance(savedGeoDistance.Value);
            }

            var geoDistance = await _calculatorService.CalculateDistanceAsync(departureIATACode, destinationIATACode, cancellationToken);

            _backgroundTaskProvider.AddBackgroundTask(c => _distancesRepository.AddDistanceAsync(departureIATACode, destinationIATACode, geoDistance.Miles, c));

            return geoDistance;
        }
    }
}
