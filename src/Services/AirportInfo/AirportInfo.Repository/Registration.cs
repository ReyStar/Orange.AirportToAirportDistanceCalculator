using System;
using AirportInfo.Common;
using AirportInfo.Domain.Interfaces;
using AirportInfo.Repository.DataBaseProducer;
using AirportInfo.Repository.DataBaseProducer.Migrations;
using AirportInfo.Repository.Infrastructure;
using AirportInfo.Repository.Repositories;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AirportInfo.Repository
{
    public static class Registration
    {
        public static IServiceCollection RegisterRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConnectionStrings>(configuration.GetSection(nameof(ConnectionStrings)));

            services.AddSingleton<IValidateOptions<ConnectionStrings>, ConnectionStringsValidator>();

            services.AddSingleton<Profile, AutoMapperProfile>();

            services.AddSingleton<IAirportInfoRepository, AirportInfoRepository>();

            services.AddSingleton<IDataSource, DataSource>();

            services.AddSingleton<IHealthCheckValidator, DataBaseHealthCheckValidator>();
            
            services.AddSingleton<IDataBaseMigrator, DataBaseMigrator>();

            services.AddSingleton<IDbMigration, AirportInformationDbMigration>();

            return services;
        }
    }
}
