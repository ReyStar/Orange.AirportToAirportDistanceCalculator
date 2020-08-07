using System.Threading;
using System.Threading.Tasks;
using DistanceCalculator.Application.Models;
using Refit;

namespace DistanceCalculator.Application.Interfaces
{
    interface IAirportInfoClient
    {
        [Get("/airports/{iataCode}")]
        Task<AirportInfo> GetAirportInfoAsync(string iataCode, CancellationToken cancellationToken);
    }
}
