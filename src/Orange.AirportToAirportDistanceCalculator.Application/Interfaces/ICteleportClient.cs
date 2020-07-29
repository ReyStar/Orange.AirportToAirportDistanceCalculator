using System.Threading;
using System.Threading.Tasks;
using Orange.AirportToAirportDistanceCalculator.Application.Models;
using Refit;

namespace Orange.AirportToAirportDistanceCalculator.Application.Interfaces
{
    interface ICteleportClient
    {
        [Get("/airports/{iataCode}")]
        Task<CteleportAirportInfo> GetAirportInfoAsync(string iataCode, CancellationToken cancellationToken);
    }
}
