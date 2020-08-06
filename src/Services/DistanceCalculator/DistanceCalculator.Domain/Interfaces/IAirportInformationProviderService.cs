using System.Threading;
using System.Threading.Tasks;
using DistanceCalculator.Domain.Models;

namespace DistanceCalculator.Domain.Interfaces
{
    /// <summary>
    /// A service that provides information about airports
    /// </summary>
    public interface IAirportInformationProviderService
    {
        /// <summary>
        /// Get airport information using IATA.
        /// </summary>
        /// <param name="IATACode">The IATA code.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        Task<AirportInformation?> GetInformationAsync(string IATACode, CancellationToken cancellationToken = default);
    }
}
