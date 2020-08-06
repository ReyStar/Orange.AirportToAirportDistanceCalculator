using DistanceCalculator.Domain.Models;

namespace DistanceCalculator.Domain.Interfaces
{
    /// <summary>
    /// Calculating the distance between two geographical coordinates
    /// </summary>
    public interface IGeoCalculatorService
    {
        /// <summary>
        /// Calculate distance in miles use geo coordinates
        /// </summary>
        double CalculateDistance(GeoCoordinate startLocation, GeoCoordinate endLocation = default);
    }
}
