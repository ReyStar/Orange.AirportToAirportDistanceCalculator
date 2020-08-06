using System.Threading;
using System.Threading.Tasks;

namespace DistanceCalculator.Application.Interfaces
{
    /// <summary>
    /// Repository for storing distances between airports
    /// </summary>
    public interface IDistancesRepository
    {
        /// <summary>
        /// Get distance from repository
        /// </summary>
        Task<double?> GetDistanceAsync(string departureIATACode, string destinationIATACode, CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Save distance to database
        /// </summary>
        Task<bool> AddDistanceAsync(string departureIATACode, string destinationIATACode, double distance, CancellationToken cancellationToken = default);
    }
}
