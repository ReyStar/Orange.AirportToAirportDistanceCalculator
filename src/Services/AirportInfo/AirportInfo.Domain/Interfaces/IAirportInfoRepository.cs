using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AirportInfo.Domain.Models;

namespace AirportInfo.Domain.Interfaces
{
    /// <summary>
    /// Repository for storing distances between airports
    /// </summary>
    public interface IAirportInfoRepository
    {
        /// <summary>
        /// Get airport information by IATA code
        /// </summary>
        Task<AirportInformation> GetAirportInfoAsync(string IATACode, CancellationToken cancellationToken = default);

        /// <summary>
        /// Add or update the airport information
        /// </summary>
        Task<bool> AddOrUpdateAirportInfoAsync(string IATACode, AirportInformation airportInformation, CancellationToken cancellationToken = default);

        /// <summary>
        /// Find the airport information use Full text search
        /// </summary>
        Task<IEnumerable<AirportInformation>> FindAirportInfoUseFullTextAsync(string fuzzyString, CancellationToken cancellationToken = default);
    }
}
