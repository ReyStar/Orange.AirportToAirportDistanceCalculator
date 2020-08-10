using AccountManager.Shell.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prometheus;

namespace AccountManager.Shell.Configuration
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
