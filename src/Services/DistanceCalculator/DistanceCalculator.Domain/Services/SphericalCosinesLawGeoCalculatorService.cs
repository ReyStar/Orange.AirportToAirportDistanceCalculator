using System;
using DistanceCalculator.Domain.Extensions;
using DistanceCalculator.Domain.Interfaces;
using DistanceCalculator.Domain.Models;

namespace DistanceCalculator.Domain.Services
{
    /// <summary>
    ///  Calculating the distance between two geographical coordinates use Spherical law of cosines
    /// </summary>
    class SphericalCosinesLawGeoCalculatorService : IGeoCalculatorService
    {
        private const double RadiusEarthMiles = 3959;

        /// <summary>
        /// Calculate distance in miles use geo coordinates
        /// </summary>
        public double CalculateDistance(GeoCoordinate startLocation, GeoCoordinate endLocation)
        {
            var radLat1 = startLocation.Latitude.ToRadian();
            var radLat2 = endLocation.Latitude.ToRadian();
            var radLon1 = startLocation.Longitude.ToRadian();
            var radLon2 = endLocation.Longitude.ToRadian();

            // central angle, aka arc segment angular distance    
            var centralAngle = Math.Acos(Math.Sin(radLat1) * Math.Sin(radLat2) +
                                         Math.Cos(radLat1) * Math.Cos(radLat2) * 
                                         Math.Cos(radLon2 - radLon1));

            return RadiusEarthMiles * centralAngle;
        }
    }
}
