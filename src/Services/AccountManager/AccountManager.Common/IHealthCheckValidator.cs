using System.Threading;
using System.Threading.Tasks;

namespace AccountManager.Common
{
    public interface IHealthCheckValidator
    {
        Task EnsureValidationAsync(CancellationToken cancellationToken = default);
    }
}
