using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;
using Routes.Shell.Infrastructure;

namespace Routes.Shell.Configuration
{
    static class MetricsConfig
    {
        public static void RegisterMetrics(this IServiceCollection services, 
                                           IConfiguration configuration)
        {
            // To register addition services for metrics configuration
           services.AddSingleton<RequestWriterMiddleware>();
        }

        public static void UseMetrics(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestWriterMiddleware>();
            app.UseHttpMetrics();
            app.UseMetricServer();
        }
    }
}
