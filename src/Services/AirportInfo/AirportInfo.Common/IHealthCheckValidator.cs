using System.Threading;
using System.Threading.Tasks;

namespace AirportInfo.Common
{
    public interface IHealthCheckValidator
    {
        Task EnsureValidationAsync(CancellationToken cancellationToken = default);
    }
}
