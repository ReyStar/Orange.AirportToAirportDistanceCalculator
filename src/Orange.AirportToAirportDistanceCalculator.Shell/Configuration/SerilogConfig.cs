using System;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Serilog;

namespace Orange.AirportToAirportDistanceCalculator.Shell.Configuration
{
    static class SerilogConfig
    {
        // <summary>
        /// Register Serilog
        /// https://dotnet-cookbook.cfapps.io/core/scoped-logging-with-serilog/
        /// https://andrewlock.net/adding-serilog-to-the-asp-net-core-generic-host/
        /// </summary>
        public static IHostBuilder RegisterSerilog(this IHostBuilder hostBuilder)
        {
            return hostBuilder.ConfigureLogging(logging => { logging.ClearProviders(); })
                .UseSerilog((context, configuration) =>
                {
                    configuration.ReadFrom.Configuration(context.Configuration)
                                 .Enrich.WithDynamicProperty("GC", () => $"{GC.CollectionCount(0)}-{GC.CollectionCount(1)}-{GC.CollectionCount(2)}")
                                 .Enrich.WithProperty("Version", PlatformServices.Default.Application.ApplicationVersion);
                }, writeToProviders: true);
        }
    }
}
