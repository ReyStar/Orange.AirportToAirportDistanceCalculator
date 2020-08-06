using System;
using System.Threading;
using System.Threading.Tasks;
using AirportInfo.Common;
using AirportInfo.Repository.Infrastructure;
using Microsoft.Extensions.Options;

namespace AirportInfo.Repository
{
    class DataBaseHealthCheckValidator: IHealthCheckValidator
    {
        private readonly IDataSource _dataSource;
        private readonly IDataBaseMigrator _dataBaseMigrator;
        private readonly ConnectionStrings _configuration;

        public DataBaseHealthCheckValidator(IDataSource dataSource, 
                                            IDataBaseMigrator dataBaseMigrator,
                                            IOptions<ConnectionStrings> configuration)
        {
            _dataSource = dataSource;
            _dataBaseMigrator = dataBaseMigrator;
            _configuration = configuration.Value;
        }

        public async Task EnsureValidationAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                // create data base if not exist
                await _dataBaseMigrator.RunAsync(cancellationToken);
            }
            catch (Exception ex) when (!(ex is HealthCheckException))
            {
                throw new HealthCheckException("Database connection error", ex);
            }
        }
    }
}
