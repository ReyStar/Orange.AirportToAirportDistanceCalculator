using System;

namespace Routes.Application.Infrastructure
{
    [Flags]
    enum CacheDataType
    {
        None = 0,
        Memory = 1,
    }
}