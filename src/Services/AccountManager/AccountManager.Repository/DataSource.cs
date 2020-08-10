using System.Data;
using AccountManager.Repository.DapperConfig.Maps;
using AccountManager.Repository.Infrastructure;
using Dapper.Dommel.FluentMapping;
using Dapper.FluentMap;
using EnsureThat;
using Microsoft.Extensions.Options;
using Npgsql;

namespace AccountManager.Repository
{
    public class DataSource: IDataSource
    {
        static DataSource()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new AccountDbModelMap());
                config.AddMap(new VersionInfoDbModelMap());
                config.AddMap(new AccessTokenDbModelMap());
                config.AddMap(new RefreshTokenDbModelMap());

                config.ApplyToDommel();
            });
        }

        private readonly ConnectionStrings _configuration;

        public DataSource(IOptions<ConnectionStrings> configuration)
        {
            EnsureArg.IsNotNull(configuration.Value, nameof(configuration.Value));
            EnsureArg.IsLte(configuration.Value.RequiredVersion, configuration.Value.RequiredVersion, nameof(configuration.Value.RequiredVersion));
            EnsureArg.IsNotNullOrWhiteSpace(configuration.Value.DefaultConnection, nameof(configuration.Value.DefaultConnection));
            
            _configuration = configuration.Value;
        }

        public IDbConnection Connection => new NpgsqlConnection(_configuration.DefaultConnection);
    }
}
