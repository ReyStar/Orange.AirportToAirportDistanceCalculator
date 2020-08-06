using MongoDB.Driver;

namespace AirportInfo.Repository
{
    internal interface IDataSource
    {
        IMongoDatabase Database { get; }
    }
}
