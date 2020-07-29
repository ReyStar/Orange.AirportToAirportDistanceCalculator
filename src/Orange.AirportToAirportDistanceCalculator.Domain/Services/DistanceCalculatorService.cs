using System;
using System.Threading;
using System.Threading.Tasks;
using Orange.AirportToAirportDistanceCalculator.Domain.Exceptions;
using Orange.AirportToAirportDistanceCalculator.Domain.Interfaces;
using Orange.AirportToAirportDistanceCalculator.Domain.Models;

namespace Orange.AirportToAirportDistanceCalculator.Domain.Services
{
    /// <summary>
    /// Service for calculating the distance between airports
    /// For improve performance can request data from DB and from external service in parallel
    /// but need understand, this approach may overload the external service
    /// </summary>
    class DistanceCalculatorService : IDistanceCalculatorService
    {
        private readonly IAirportInformationProviderService _airportInformationProviderService;
        private readonly IGeoCalculatorService _geoCalculatorService;

        /// <summary>Initializes a new instance of the <see cref="DistanceCalculatorService" /> class.</summary>
        /// <param name="airportInformationProviderService">The airport information provider service.</param>
        /// <param name="geoCalculatorService">The geo calculator service.</param>
        public DistanceCalculatorService(IAirportInformationProviderService airportInformationProviderService,
                                         IGeoCalculatorService geoCalculatorService)
        {
            _airportInformationProviderService = airportInformationProviderService;
            _geoCalculatorService = geoCalculatorService;
        }

        /// <summary>
        /// Calculate the distance between two airports.
        /// </summary>
        public async Task<GeoDistance> CalculateDistanceAsync(string departureIATACode, string destinationIATACode, CancellationToken cancellationToken)
        {
            // Special case if the departure airport is equal to the destination airport
            // in this place don't IATA code exists
            if (string.Equals(departureIATACode, destinationIATACode,
                StringComparison.OrdinalIgnoreCase))
            {
                return new GeoDistance(0.0);
            }

            var getDepartureInfoTask =
                _airportInformationProviderService.GetInformationAsync(departureIATACode, cancellationToken);

            var getDestinationInfoTask =
                _airportInformationProviderService.GetInformationAsync(destinationIATACode, cancellationToken);

            await Task.WhenAll(getDepartureInfoTask, getDestinationInfoTask);

            var departureInfo = await getDepartureInfoTask;
            var destinationInfo = await getDestinationInfoTask;

            if (departureInfo == null)
            {
                throw new AirportNotFoundException($"Departure airport {departureIATACode} information not found");
            }

            if (destinationInfo == null)
            {
                throw new AirportNotFoundException($"Destination airport {destinationIATACode} information not found");
            }

            var geoDistance =
                _geoCalculatorService.CalculateDistance(departureInfo.Value.Location, destinationInfo.Value.Location);

            return new GeoDistance(geoDistance);
        }
    }
}
