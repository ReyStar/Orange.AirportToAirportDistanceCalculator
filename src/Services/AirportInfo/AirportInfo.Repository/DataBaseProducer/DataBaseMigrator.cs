using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AirportInfo.Common;
using AirportInfo.Repository.DataBaseProducer.Migrations;
using AirportInfo.Repository.Infrastructure;
using AirportInfo.Repository.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace AirportInfo.Repository.DataBaseProducer
{
    class DataBaseMigrator: IDataBaseMigrator
    {
        private readonly IEnumerable<IDbMigration> _migrations;
        private readonly IDataSource _dataSource;
        private readonly ILogger _logger;
        private readonly ConnectionStrings _connectionString;

        public DataBaseMigrator(IOptions<ConnectionStrings> connectionStrings,
                               IEnumerable<IDbMigration> migrations,
                               IDataSource dataSource,
                               ILogger<DataBaseMigrator> logger)
        {
            _migrations = migrations;
            _dataSource = dataSource;
            _logger = logger;
            _connectionString = connectionStrings.Value;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Checking the database");

            var collection = _dataSource.Database.GetCollection<VersionInfoDbModel>("VersionInfo");
            var currentVersion = 0;

            if (!await collection.AsQueryable().AnyAsync(cancellationToken: cancellationToken))
            {
                var builder = Builders<VersionInfoDbModel>.IndexKeys;
                var keys = builder.Descending(prop => prop.Version);

                var indexName = await collection.Indexes.CreateOneAsync(new CreateIndexModel<VersionInfoDbModel>(keys),
                    new CreateOneIndexOptions
                    {

                    },
                    cancellationToken: cancellationToken);
            }
            else
            {
                var cursorToResults = collection.Find<VersionInfoDbModel>(model => model.Version > 0)
                                                .Sort(new SortDefinitionBuilder<VersionInfoDbModel>().Descending(x => x.Version));
                
                var lastVersionInfoDbModel = await cursorToResults.FirstOrDefaultAsync(cancellationToken);
                if (lastVersionInfoDbModel != null)
                {
                    currentVersion = lastVersionInfoDbModel.Version;
                }
            }

            if (_connectionString.RequiredVersion < currentVersion)
            {
                throw new DataBaseMigrationException($"The requested database version {_connectionString.RequiredVersion} is less than the current one {currentVersion}");
            }

            // Execute the migrations
            var migrations = _migrations.Where(x => x.Version > currentVersion && x.Version <= _connectionString.RequiredVersion)
                                        .OrderBy(x => x.Version)
                                        .ToList();

            if (migrations.Any())
            {
                _logger.LogDebug("Database migration has started");

                foreach (var dbMigration in migrations)
                {
                    await dbMigration.UpAsync(cancellationToken);
                    currentVersion = dbMigration.Version;
                    await collection.InsertOneAsync(
                        new VersionInfoDbModel(currentVersion, DateTime.UtcNow, $"Migration {dbMigration.Version}"),
                        new InsertOneOptions(), cancellationToken: cancellationToken);
                }

                _logger.LogInformation("Database migration completed");
                _logger.LogInformation($"Current database version {currentVersion}");
            }
        }
    }
}
