using System.Threading;
using System.Threading.Tasks;

namespace AccountManager.Common
{
    public interface IDataBaseCreator
    {
        Task RunAsync(CancellationToken cancellationToken = default);
    }
}
