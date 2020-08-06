using DistanceCalculator.Domain.Interfaces;
using DistanceCalculator.Domain.Models;
using Geodesy;

namespace DistanceCalculator.Domain.Services
{
    /// <summary>
    /// Calculating the distance between two geographical coordinates use Vincenty's algorithm
    /// </summary>
    class VincentGeoCalculatorService : IGeoCalculatorService
    {
        private static GeodeticCalculator _geodeticCalculator = new GeodeticCalculator(Ellipsoid.WGS84);

        /// <summary>
        /// Calculate distance in miles use geo coordinates
        /// </summary>
        public double CalculateDistance(GeoCoordinate startLocation, GeoCoordinate endLocation)
        {
            var geodeticCurve = _geodeticCalculator.CalculateGeodeticCurve(new GlobalCoordinates(startLocation.Latitude, startLocation.Longitude),
                                                                          new GlobalCoordinates(endLocation.Latitude, endLocation.Longitude));
            return geodeticCurve.EllipsoidalDistance * 0.000621371192;
        }
    }
}
