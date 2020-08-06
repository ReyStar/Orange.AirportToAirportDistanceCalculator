using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Prometheus;

namespace DistanceCalculator.Shell.Infrastructure
{
    /// <summary>
    /// Write request metrics
    /// </summary>
    class RequestWriterMiddleware : IMiddleware
    {
        private readonly ILogger _logger;
        private static readonly Counter HttpRequestCounter;
        private static readonly Gauge HttpRequestGauge;
        private static readonly Histogram HttpRequestHistogram;

        static RequestWriterMiddleware()
        {
            var labels = new[] {"path", "method", "status", "controller", "action"};

            HttpRequestCounter = Metrics.CreateCounter("request_total", "HTTP Requests count meter",
                new CounterConfiguration
                {
                    LabelNames = labels
                });

            HttpRequestGauge = Metrics.CreateGauge("request_sum", "HTTP Requests gauge meter",
                new GaugeConfiguration
                {
                    LabelNames = labels
                });
            
            HttpRequestHistogram = Metrics
                .CreateHistogram("request_bucket", "Histogram of request",
                    new HistogramConfiguration
                    {
                        Buckets = Histogram.ExponentialBuckets(0.001, 2, 16), 
                        LabelNames = labels
                    });
        }

        /// <summary>
        /// .ctor
        /// </summary>
        public RequestWriterMiddleware(ILogger<RequestWriterMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext, RequestDelegate next)
        {
           

            var sw = Stopwatch.StartNew();
            try
            {
                await next.Invoke(httpContext);
            }
            finally
            {
                sw.Stop();
               
                
                RequestMetric(httpContext, sw.Elapsed.TotalSeconds);
            }
        }

        private void RequestMetric(HttpContext httpContext, double totalMilliseconds)
        {
            var routeData = httpContext.GetRouteData().Values ?? new RouteValueDictionary();
            var path = httpContext.Request.Path.Value;
            var method = httpContext.Request.Method;
            var statusCode = httpContext.Response.StatusCode;
            var controllerName = GetRouteValues(routeData, "Controller");
            var actionName = GetRouteValues(routeData, "Action");

            HttpRequestCounter.Labels(path, method, statusCode.ToString(), controllerName, actionName).Inc();

            HttpRequestGauge.Labels(path, method, statusCode.ToString(), controllerName, actionName).Inc();

            HttpRequestHistogram.Labels(path, method, statusCode.ToString(), controllerName, actionName).Observe(totalMilliseconds);

            _logger.LogDebug($"{method} {path} {statusCode} {controllerName} {actionName} {totalMilliseconds}");
        }

        private static string GetRouteValues(RouteValueDictionary routeData, string key)
        {
            if (routeData.TryGetValue(key, out var rawValue) 
                && rawValue is string stringValue)
            {
                return stringValue;
            }

            return string.Empty;
        }
    }
}
