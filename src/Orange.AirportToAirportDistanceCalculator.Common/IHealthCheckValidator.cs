using System.Threading;
using System.Threading.Tasks;

namespace Orange.AirportToAirportDistanceCalculator.Common
{
    public interface IHealthCheckValidator
    {
        Task EnsureValidationAsync(CancellationToken cancellationToken = default);
    }
}
