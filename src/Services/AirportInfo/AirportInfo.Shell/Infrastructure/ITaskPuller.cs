using System;
using System.Threading;
using System.Threading.Tasks;

namespace AirportInfo.Shell.Infrastructure
{
    public interface ITaskPuller
    {
        Task<Func<CancellationToken, Task>> GetTaskAsync(CancellationToken cancellationToken);
    }
}
