using System;

namespace AccountManager.Application.Infrastructure
{
    [Flags]
    enum CacheDataType
    {
        None = 0,
        Memory = 1
    }
}
