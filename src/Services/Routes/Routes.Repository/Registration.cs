using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Routes.Common;
using Routes.Domain.Interfaces;
using Routes.Repository.DataBaseProducer;
using Routes.Repository.DataBaseProducer.Migrations;
using Routes.Repository.Infrastructure;
using Routes.Repository.Repositories;

namespace Routes.Repository
{
    public static class Registration
    {
        public static IServiceCollection RegisterRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConnectionStrings>(configuration.GetSection(nameof(ConnectionStrings)));

            services.AddSingleton<IValidateOptions<ConnectionStrings>, ConnectionStringsValidator>();

            services.AddSingleton<Profile, AutoMapperProfile>();

            services.AddSingleton<IRoutesRepository, RoutesRepository>();

            services.AddSingleton<IDataSource, DataSource>();

            services.AddSingleton<IHealthCheckValidator, DataBaseHealthCheckValidator>();
            
            services.AddSingleton<IDataBaseMigrator, DataBaseMigrator>();

            services.AddSingleton<IDbMigration, RouteDbMigration>();

            return services;
        }
    }
}
