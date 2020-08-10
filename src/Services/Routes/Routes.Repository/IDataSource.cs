using MongoDB.Driver;

namespace Routes.Repository
{
    internal interface IDataSource
    {
        IMongoDatabase Database { get; }
    }
}
