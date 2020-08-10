using System.Threading;
using System.Threading.Tasks;

namespace Routes.Common
{
    public interface IHealthCheckValidator
    {
        Task EnsureValidationAsync(CancellationToken cancellationToken = default);
    }
}
