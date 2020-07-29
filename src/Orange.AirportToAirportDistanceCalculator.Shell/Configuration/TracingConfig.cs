using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Util;

namespace Orange.AirportToAirportDistanceCalculator.Shell.Configuration
{
    static class TracingConfig
    {
        public static void RegisterTracing(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOpenTracing();

            // Adds the Jaeger Tracer.
            services.AddSingleton<ITracer>(serviceProvider =>
            {
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();

                var config = Jaeger.Configuration.FromIConfiguration(loggerFactory, configuration.GetSection("Jaeger"));
                
                var tracer = config.GetTracer();
                
                // Allows code that can't use DI to also access the tracer.
                GlobalTracer.Register(tracer);

                return tracer;
            });
        }
    }
}
