using System;

namespace AirportInfo.Application.Infrastructure
{
    [Flags]
    enum CacheDataType
    {
        None = 0,
        Memory = 1,
    }
}