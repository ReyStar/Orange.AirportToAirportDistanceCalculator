using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Routes.Repository.Models;

namespace Routes.Repository.DataBaseProducer.Migrations
{
    abstract class DbMigration<TCollection> : IDbMigration
        where TCollection: MongoDbModel
    {
        protected DbMigration(int version, string description, IDataSource dataSource, ILogger logger)
        {
            Version = version;
            Logger = logger;
            DataSource = dataSource;
            Description = description;
        }

        protected ILogger Logger { get; }

        protected IDataSource DataSource { get; }

        public Type CollectionType => typeof(TCollection);

        public int Version { get; }
        
        public string Description { get; }

        public Task UpAsync(CancellationToken cancellationToken = default)
        {
            return OnUpAsync(cancellationToken);
        }

        protected abstract Task OnUpAsync(CancellationToken cancellationToken);
    }
}
