using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Routes.Common;
using Routes.Repository.Infrastructure;

namespace Routes.Repository
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
