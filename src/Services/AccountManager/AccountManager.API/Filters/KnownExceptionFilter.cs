﻿using System;
using System.Net;
using AccountManager.Domain.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AccountManager.API.Filters
{
    /// <summary>
    /// Filter for catch known domain exception
    /// </summary>
    public class KnownExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger _logger;

        /// <summary>
        /// .ctor
        /// </summary>
        public KnownExceptionFilter(IWebHostEnvironment env, ILogger<KnownExceptionFilter> logger)
        {
            _env = env;
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is AccountAuthenticationException accountAuthenticationException)
            {
                WriteException(accountAuthenticationException, HttpStatusCode.BadRequest, context);
            }
            else
            if (context.Exception is AccountManagerException accountManagerException)
            {
                WriteException(accountManagerException, HttpStatusCode.BadRequest, context);
            }
        }
  
        private void WriteException<T>(T ex, HttpStatusCode statusCode, ExceptionContext context)
            where T : Exception
        {
            _logger.LogError($"{nameof(T)}: {ex.Message}");

            context.ExceptionHandled = true;

            if (_env.IsDevelopment())
            {
                context.Result = new ContentResult
                {
                    StatusCode = (int) statusCode,
                    Content = JsonConvert.SerializeObject(ex),
                    ContentType = "application/json"
                };
                context.HttpContext.Response.Headers.Add("reason-phrase", ex.Message);
            }
            else
            {
                context.Result = new StatusCodeResult((int) statusCode);
            }
        }
    }
}
