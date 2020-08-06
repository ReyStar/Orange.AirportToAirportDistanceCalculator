using System;
using System.Threading;
using System.Threading.Tasks;

namespace AirportInfo.Repository.DataBaseProducer.Migrations
{
    interface IDbMigration
    {
        Type CollectionType { get; }

        int Version { get; }

        string Description { get; }

        Task UpAsync(CancellationToken cancellationToken = default);
    }
}
