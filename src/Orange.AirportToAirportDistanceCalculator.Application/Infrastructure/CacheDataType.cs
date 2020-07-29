using System;

namespace Orange.AirportToAirportDistanceCalculator.Application.Infrastructure
{
    [Flags]
    enum CacheDataType
    {
        None = 0,
        Memory = 1,
        DataBase = 2,
        All = Memory | DataBase
    }
}
