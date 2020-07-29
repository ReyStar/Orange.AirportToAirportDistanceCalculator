using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using Orange.AirportToAirportDistanceCalculator.Common;
using Orange.AirportToAirportDistanceCalculator.Repository.Infrastructure;
using Orange.AirportToAirportDistanceCalculator.Repository.Models;

namespace Orange.AirportToAirportDistanceCalculator.Repository
{
    class DataBaseHealthCheckValidator: IHealthCheckValidator
    {
        private readonly IDataSource _dataSource;
        private readonly IDataBaseCreator _dataBaseCreator;
        private readonly ConnectionStrings _configuration;

        public DataBaseHealthCheckValidator(IDataSource dataSource, 
                                            IDataBaseCreator dataBaseCreator,
                                            IOptions<ConnectionStrings> configuration)
        {
            _dataSource = dataSource;
            _dataBaseCreator = dataBaseCreator;
            _configuration = configuration.Value;
        }

        public async Task EnsureValidationAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                // create data base if not exist
                _dataBaseCreator.Run();

                //Check data schema version on start
                var versionInfo = await _dataSource.Connection.QueryFirstOrDefaultAsync<VersionInfoDbModel>(
                    new CommandDefinition("SELECT Version, AppliedOn, Description FROM VersionInfo ORDER BY Version DESC LIMIT 1;",
                        cancellationToken: cancellationToken));

                if (versionInfo.Version != _configuration.RequiredVersion)
                {
                    throw new HealthCheckException($"Request database version {_configuration.RequiredVersion}, but current {versionInfo.Version}");
                }
            }
            catch (Exception ex) when (!(ex is HealthCheckException))
            {
                throw new HealthCheckException("Database connection error", ex);
            }
        }
    }
}
