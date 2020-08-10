using System.Threading;
using System.Threading.Tasks;

namespace Routes.Common
{
    public interface IDataBaseMigrator
    {
        Task RunAsync(CancellationToken cancellationToken = default);
    }
}
