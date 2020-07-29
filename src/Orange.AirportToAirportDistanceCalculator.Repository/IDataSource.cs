using System.Data;

namespace Orange.AirportToAirportDistanceCalculator.Repository
{
    internal interface IDataSource
    {
        IDbConnection Connection { get; }
    }
}
