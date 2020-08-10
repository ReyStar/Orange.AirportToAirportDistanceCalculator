using AccountManager.Common;
using AccountManager.Shell.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AccountManager.Shell
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
