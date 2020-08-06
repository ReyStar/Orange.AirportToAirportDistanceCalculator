using System.Threading;
using System.Threading.Tasks;
using DistanceCalculator.Domain.Models;

namespace DistanceCalculator.Domain.Interfaces
{
    /// <summary>
    /// Service for calculating the distance between airports
    /// </summary>
    public interface IDistanceCalculatorService
    {
        /// <summary>
        /// Calculate the distance between two airports.
        /// </summary>
        /// <param name="departureIATACode">Departure airport IATA airport code is a three-letter geocode</param>
        /// <param name="destinationIATACode">Destination airport IATA airport code is a three-letter geocode</param>
        Task<GeoDistance> CalculateDistanceAsync(string departureIATACode, string destinationIATACode, CancellationToken cancellationToken = default);
    }
}
