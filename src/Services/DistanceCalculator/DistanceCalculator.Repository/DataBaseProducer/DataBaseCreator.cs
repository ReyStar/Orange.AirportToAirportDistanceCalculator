using System;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using DistanceCalculator.Common;
using DistanceCalculator.Repository.Infrastructure;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DistanceCalculator.Repository.DataBaseProducer
{
    class DataBaseCreator: IDataBaseCreator
    {
        private readonly IServiceProvider _services;
        private readonly ILogger _logger;
        private readonly string _connectionString;
        private const string ConnectionStringName = "DefaultConnection";
        private const string DataSourceConstName = "Data Source";
        private const string MemoryDataSourceConstName = ":memory:";

        public DataBaseCreator(IOptions<ConnectionStrings> connectionStrings,
                               IServiceProvider services,
                               ILogger<DataBaseCreator> logger)
        {
            _services = services;
            _logger = logger;
            _connectionString = connectionStrings.Value.DefaultConnection;

            EnsureThat.EnsureArg.IsNotNullOrWhiteSpace(_connectionString, ConnectionStringName);
        }

        public void Run()
        {
            var builder = new DbConnectionStringBuilder
            {
                ConnectionString = _connectionString
            };

            if (!builder.TryGetValue(DataSourceConstName, out var dataSourceObjectValue)
                || !(dataSourceObjectValue is string dataSource)
                || string.IsNullOrWhiteSpace(dataSource))
            {
                throw new ArgumentException($"Database connection string must contain not empty {DataSourceConstName}");
            }

            if (dataSource.Equals(MemoryDataSourceConstName, StringComparison.OrdinalIgnoreCase))
            {
                throw new ArgumentException($"{DataSourceConstName} can't be {MemoryDataSourceConstName}");
            }

            if (!File.Exists(dataSource))
            {
                SQLiteConnection.CreateFile(dataSource);
                _logger.LogInformation($"Database was created {dataSource}");
            }

            // Execute the migrations
            // Scope by default
            using (var serviceScope = _services.CreateScope())
            {
                var migrationRunner = serviceScope.ServiceProvider.GetService<IMigrationRunner>();
                migrationRunner.MigrateUp();
            } 
        }
    }
}
