using System;
using Orange.AirportToAirportDistanceCalculator.Domain.Extensions;
using Orange.AirportToAirportDistanceCalculator.Domain.Interfaces;
using Orange.AirportToAirportDistanceCalculator.Domain.Models;

namespace Orange.AirportToAirportDistanceCalculator.Domain.Services
{
    /// <summary>
    /// Calculating the distance between two geographical coordinates use Haversine formula
    /// This approach can be optimized https://www.youtube.com/watch?v=pTZqP78YYIA
    /// </summary>
    public class HaversineGeoCalculatorService : IGeoCalculatorService
    {
        /// <summary>
        /// Calculate distance in miles use geo coordinates
        /// </summary>
        public double CalculateDistance(GeoCoordinate startLocation, GeoCoordinate endLocation)
        {
            var latitude = (endLocation.Latitude - startLocation.Latitude).ToRadian();
            var longitude = (endLocation.Longitude - startLocation.Longitude).ToRadian();
            var angle = Math.Sin(latitude / 2) * Math.Sin(latitude / 2) +
                        Math.Cos((startLocation.Latitude).ToRadian()) * Math.Cos(endLocation.Latitude.ToRadian()) *
                        Math.Sin(longitude / 2) * Math.Sin(longitude / 2);
            
            return 7920 * Math.Asin(Math.Min(1, Math.Sqrt(angle)));
        }
    }
}