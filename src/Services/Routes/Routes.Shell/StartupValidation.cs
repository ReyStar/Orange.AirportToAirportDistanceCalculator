using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Routes.Common;

namespace Routes.Shell
{
    public class StartupValidation : IStartupFilter
    {
        private readonly IEnumerable<IHealthCheckValidator> _validators;

        public StartupValidation(IEnumerable<IHealthCheckValidator> validators)
        {
            _validators = validators;
        }

        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            foreach (var validator in _validators)
            {
                validator.EnsureValidationAsync()
                         .GetAwaiter()
                         .GetResult();
            }

            return next;
        }
    }
}
