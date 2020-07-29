using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Orange.AirportToAirportDistanceCalculator.API.Filters
{
    /// <summary>
    /// Log connection cancelled exception as a 499
    /// (when user cancelled request)
    /// https://andrewlock.net/using-cancellationtokens-in-asp-net-core-mvc-controllers/
    /// </summary>
    public class OperationCancelledExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger _logger;

        /// <summary>
        /// .ctor
        /// </summary>
        public OperationCancelledExceptionFilter(ILogger<OperationCancelledExceptionFilter> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// On throw exception
        /// </summary>
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is OperationCanceledException)
            {
                _logger.LogInformation("Request was cancelled");
                
                context.ExceptionHandled = true;
                context.Result = new StatusCodeResult(499);
            }
        }
    }
}
