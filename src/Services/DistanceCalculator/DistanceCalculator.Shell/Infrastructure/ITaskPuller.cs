using System;
using System.Threading;
using System.Threading.Tasks;

namespace DistanceCalculator.Shell.Infrastructure
{
    public interface ITaskPuller
    {
        Task<Func<CancellationToken, Task>> GetTaskAsync(CancellationToken cancellationToken);
    }
}
