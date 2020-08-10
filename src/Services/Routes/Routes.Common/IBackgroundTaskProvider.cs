using System;
using System.Threading;
using System.Threading.Tasks;

namespace Routes.Common
{
    public interface IBackgroundTaskProvider
    {
        void AddBackgroundTask(Func<CancellationToken, Task> workItem);
    }
}
