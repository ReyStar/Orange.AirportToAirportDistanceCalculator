using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace DistanceCalculator.Shell.Infrastructure
{
    /// <summary>
    /// Service for requesting the service health status.
    /// now it's the plug should be improved at the request of the operation team
    /// </summary>
    class HealthCheck : IHealthCheck
    {
        private readonly ILogger _logger;

        public HealthCheck(ILogger<HealthCheck> logger)
        {
            _logger = logger;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
                                                        CancellationToken cancellationToken = default)
        {
            return Task.FromResult(new HealthCheckResult(HealthStatus.Healthy));
        }
    }
}
