using System.Threading;
using System.Threading.Tasks;

namespace DistanceCalculator.Common
{
    public interface IHealthCheckValidator
    {
        Task EnsureValidationAsync(CancellationToken cancellationToken = default);
    }
}
