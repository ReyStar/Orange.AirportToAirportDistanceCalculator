﻿using System.Data;
using System.Data.SQLite;
using Dapper.Dommel.FluentMapping;
using Dapper.FluentMap;
using EnsureThat;
using Microsoft.Extensions.Options;
using Orange.AirportToAirportDistanceCalculator.Repository.DapperConfig.Maps;
using Orange.AirportToAirportDistanceCalculator.Repository.Infrastructure;

namespace Orange.AirportToAirportDistanceCalculator.Repository
{
    public class DataSource: IDataSource
    {
        static DataSource()
        {
            FluentMapper.Initialize(config =>
            {
                config.AddMap(new GeoDistanceDbModelMap());
                config.AddMap(new VersionInfoDbModelMap());
                config.ApplyToDommel();
            });
        }

        private readonly ConnectionStrings _configuration;

        public DataSource(IOptions<ConnectionStrings> configuration)
        {
            EnsureArg.IsNotNull(configuration.Value, nameof(configuration.Value));
            EnsureArg.IsLte(configuration.Value.RequiredVersion, 1, nameof(configuration.Value.RequiredVersion));
            EnsureArg.IsNotNullOrWhiteSpace(configuration.Value.DefaultConnection, nameof(configuration.Value.DefaultConnection));
            
            _configuration = configuration.Value;
        }

        public IDbConnection Connection => new SQLiteConnection(_configuration.DefaultConnection);
    }
}
