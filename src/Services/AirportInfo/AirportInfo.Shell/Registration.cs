using AirportInfo.Common;
using AirportInfo.Shell.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AirportInfo.Shell
{
    internal static class Registration
    {
        public static void RegisterShellDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHostedService<QueuedHostedService>();
            services.AddSingleton<BackgroundTaskProvider>();
            services.AddSingleton<IBackgroundTaskProvider>(x => x.GetRequiredService<BackgroundTaskProvider>()); 
            services.AddSingleton<ITaskPuller>(x => x.GetRequiredService<BackgroundTaskProvider>()); 
        }
    }
}
