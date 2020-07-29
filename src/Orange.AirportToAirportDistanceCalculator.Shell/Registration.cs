using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orange.AirportToAirportDistanceCalculator.Common;
using Orange.AirportToAirportDistanceCalculator.Shell.Infrastructure;

namespace Orange.AirportToAirportDistanceCalculator.Shell
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
