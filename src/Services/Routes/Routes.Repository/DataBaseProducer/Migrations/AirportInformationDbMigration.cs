using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Routes.Repository.Models;
using Routes.Repository.Repositories;

namespace Routes.Repository.DataBaseProducer.Migrations
{
    /// <summary>
    /// Migration airport collection V1
    /// </summary>
    class RouteDbMigration : DbMigration<RouteDbModel>
    {
        private readonly IMapper _mapper;

        public RouteDbMigration(IDataSource dataSource, IMapper mapper, ILogger<RouteDbMigration> logger)
            : base(1, "Initializing the routes collection", dataSource, logger)
        {
            _mapper = mapper;
        }

        protected override async Task OnUpAsync(CancellationToken cancellationToken)
        {
            Logger.LogInformation($"{nameof(RouteDbMigration)} has started");

            var collection = DataSource.Database.GetCollection<RouteDbModel>(RoutesRepository.CollectionName);

            if (!await collection.AsQueryable().AnyAsync(cancellationToken: cancellationToken))
            {
                // Set location indexes
                var builder = Builders<RouteDbModel>.IndexKeys;
                var userIdIndex = builder.Ascending(x => x.UserId);
                
                await collection.Indexes.CreateManyAsync(new[]
                {
                    new CreateIndexModel<RouteDbModel>(userIdIndex, new CreateIndexOptions {Name = $"{nameof(userIdIndex)}"}),

                }, cancellationToken);
            }

            Logger.LogInformation($"{nameof(RouteDbMigration)} completed");
        }
    }
}
