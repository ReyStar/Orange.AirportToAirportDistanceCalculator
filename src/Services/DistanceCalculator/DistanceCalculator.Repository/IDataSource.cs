using System.Data;

namespace DistanceCalculator.Repository
{
    internal interface IDataSource
    {
        IDbConnection Connection { get; }
    }
}
