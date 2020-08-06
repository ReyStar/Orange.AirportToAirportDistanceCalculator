using System;
using System.Threading;
using System.Threading.Tasks;

namespace DistanceCalculator.Common
{
    public interface IBackgroundTaskProvider
    {
        void AddBackgroundTask(Func<CancellationToken, Task> workItem);
    }
}
