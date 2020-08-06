using System.Threading;
using System.Threading.Tasks;
using DistanceCalculator.Application.Models;
using Refit;

namespace DistanceCalculator.Application.Interfaces
{
    interface ICteleportClient
    {
        [Get("/airports/{iataCode}")]
        Task<CteleportAirportInfo> GetAirportInfoAsync(string iataCode, CancellationToken cancellationToken);
    }
}
