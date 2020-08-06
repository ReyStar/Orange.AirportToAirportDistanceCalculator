using System;
using AutoMapper;
using DistanceCalculator.Application.Interfaces;
using DistanceCalculator.Common;
using DistanceCalculator.Repository.DataBaseProducer;
using DistanceCalculator.Repository.Infrastructure;
using DistanceCalculator.Repository.Repositories;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace DistanceCalculator.Repository
{
    public static class Registration
    {
        public static IServiceCollection RegisterRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConnectionStrings>(configuration.GetSection(nameof(ConnectionStrings)));

            services.AddSingleton<IValidateOptions<ConnectionStrings>, ConnectionStringsValidator>();

            services.AddSingleton<Profile, AutoMapperProfile>();

            services.AddSingleton<IDistancesRepository, DistancesRepository>();

            services.AddSingleton<IDataSource, DataSource>();

            services.AddSingleton<IHealthCheckValidator, DataBaseHealthCheckValidator>();
            
            services.AddFluentMigratorCore()
                .ConfigureRunner(rb =>
                {
                    // Add SqlLite support to FluentMigrators
                    rb.AddSQLite();

                    var connectionString = configuration.GetConnectionString("DefaultConnection");

                    if (string.IsNullOrWhiteSpace(connectionString))
                    {
                        throw new ArgumentException("DefaultConnection can't be null or empty");
                    }

                    // Set the connection string
                    rb.WithGlobalConnectionString(connectionString);

                    // Define the assembly containing the migrations
                    rb.ScanIn(typeof(Registration).Assembly).For.Migrations();
                })
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .Configure<RunnerOptions>(opt => { opt.TransactionPerSession = true; });

            services.AddSingleton<IDataBaseCreator, DataBaseCreator>();

            return services;
        }
    }
}
