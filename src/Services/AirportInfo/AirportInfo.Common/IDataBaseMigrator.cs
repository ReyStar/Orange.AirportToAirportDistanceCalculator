using System.Threading;
using System.Threading.Tasks;

namespace AirportInfo.Common
{
    public interface IDataBaseMigrator
    {
        Task RunAsync(CancellationToken cancellationToken = default);
    }
}
