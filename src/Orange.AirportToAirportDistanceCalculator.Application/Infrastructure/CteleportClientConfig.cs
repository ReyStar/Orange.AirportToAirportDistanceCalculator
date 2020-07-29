using System;

namespace Orange.AirportToAirportDistanceCalculator.Application.Infrastructure
{
    class CteleportClientConfig
    {
        public string BaseAddress { get; set; }

        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(100);

        public int RetryCount { get; set; } = 3;
        
        /// <summary>
        /// For timeout good approach use p50 + p99 and two retry
        /// </summary>
        public TimeSpan RetryTimeout { get; set; } = TimeSpan.FromSeconds(1);

        public int CircuitBreakerFailTryCount { get; set; } = 5;
        
        public TimeSpan CircuitTimeout { get; set; } = TimeSpan.FromSeconds(5);

        public TimeSpan HandlerLifetime { get; set; } = TimeSpan.FromMinutes(1);
    }
}
