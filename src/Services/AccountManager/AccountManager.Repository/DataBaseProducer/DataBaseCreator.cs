using System;
using System.Threading;
using System.Threading.Tasks;
using AccountManager.Common;
using AccountManager.Repository.Infrastructure;
using Dapper;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

namespace AccountManager.Repository.DataBaseProducer
{
    class DataBaseCreator: IDataBaseCreator
    {
        private readonly IServiceProvider _services;
        private readonly ILogger _logger;
        private readonly string _connectionString;
        private const string ConnectionStringName = "DefaultConnection";

        public DataBaseCreator(IOptions<ConnectionStrings> connectionStrings,
                               IServiceProvider services,
                               ILogger<DataBaseCreator> logger)
        {
            _services = services;
            _logger = logger;
            _connectionString = connectionStrings.Value.DefaultConnection;

            EnsureThat.EnsureArg.IsNotNullOrWhiteSpace(_connectionString, ConnectionStringName);
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder(_connectionString);
            var dataBase = connectionStringBuilder.Database;
            connectionStringBuilder.Database = null;

            await using var connection = new NpgsqlConnection(connectionStringBuilder.ToString());

            var isExist =
                await connection.ExecuteScalarAsync<bool?>(new CommandDefinition(
                    $"SELECT (datname IS NOT NULL) FROM pg_database WHERE datname='{dataBase}'",
                    cancellationToken: cancellationToken));

            if (!isExist.HasValue
                || !isExist.Value)
            {
                var resourceLoader = new ResourceLoader(typeof(DataBaseCreator).Assembly);
                var dbCreationScript = await resourceLoader.LoadStringAsync($"{typeof(DataBaseCreator).Namespace}.Scripts.CreateAccountManagerDB.sql");
                await connection.QueryAsync(new CommandDefinition(string.Format(dbCreationScript, dataBase, connectionStringBuilder.Username),
                                            cancellationToken: cancellationToken));

                _logger.LogInformation($"{dataBase} was created");
            }

            await connection.CloseAsync();

            // Execute the migrations
            // Scope by default
            using var serviceScope = _services.CreateScope();
            var migrationRunner = serviceScope.ServiceProvider.GetService<IMigrationRunner>();
            migrationRunner.MigrateUp();
        }
    }
}
