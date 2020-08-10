using AirportInfo.Repository.Infrastructure;
using EnsureThat;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AirportInfo.Repository
{
    public class DataSource: IDataSource
    {
        public DataSource(IOptions<ConnectionStrings> configuration)
        {
            EnsureArg.IsNotNull(configuration.Value, nameof(configuration.Value));
            EnsureArg.IsLte(configuration.Value.RequiredVersion, configuration.Value.RequiredVersion, nameof(configuration.Value.RequiredVersion));
            EnsureArg.IsNotNullOrWhiteSpace(configuration.Value.DefaultConnection, nameof(configuration.Value.DefaultConnection));

            var connectionStrings = configuration.Value;
            var client = new MongoClient(connectionStrings.DefaultConnection);

            Database = client.GetDatabase(connectionStrings.DataBase);
        }

        public IMongoDatabase Database { get; }
    }
}
