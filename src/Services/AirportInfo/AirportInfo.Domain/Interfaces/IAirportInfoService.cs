using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AirportInfo.Domain.Models;

namespace AirportInfo.Domain.Interfaces
{
    /// <summary>
    /// Service for return airports information
    /// </summary>
    public interface IAirportInfoService
    {
        /// <summary>
        /// Get airport information by IATA code
        /// </summary>
        Task<AirportInformation> GetAirportInfoAsync(string IATACode, CancellationToken cancellationToken = default);

        /// <summary>
        /// Add or update airport information
        /// </summary>
        Task AddOrUpdateAirportInfoAsync(string IATACode, AirportInformation airportInformation, CancellationToken cancellationToken = default);

        /// <summary>
        ///  Find the airport information use full text search
        /// </summary>
        Task<IEnumerable<AirportInformation>> FindAirportInfoUseFullTextSearchAsync(string query, CancellationToken cancellationToken = default);
    }
}
