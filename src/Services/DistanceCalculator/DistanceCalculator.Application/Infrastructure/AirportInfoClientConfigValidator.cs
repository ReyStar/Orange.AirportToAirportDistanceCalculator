using System;
using Microsoft.Extensions.Options;

namespace DistanceCalculator.Application.Infrastructure
{
    class AirportInfoClientConfigValidator : IValidateOptions<AirportInfoClientConfig>
    {
        public ValidateOptionsResult Validate(string name, AirportInfoClientConfig options)
        {
            if (options == null)
            {
                return ValidateOptionsResult.Fail($"The {nameof(options)} can't be null");
            }

            if (string.IsNullOrWhiteSpace(options.BaseAddress))
            {
                return ValidateOptionsResult.Fail($"The {nameof(options.BaseAddress)} must be greater than zero");
            }

            if (options.CircuitBreakerFailTryCount <= 0)
            {
                return ValidateOptionsResult.Fail($"The {nameof(options.CircuitBreakerFailTryCount)} must be greater than zero");
            }

            if (options.CircuitTimeout <= TimeSpan.Zero)
            {
                return ValidateOptionsResult.Fail($"The {nameof(options.CircuitTimeout)} must be greater than zero");
            }

            if (options.HandlerLifetime <= TimeSpan.Zero)
            {
                return ValidateOptionsResult.Fail($"The {nameof(options.HandlerLifetime)} must be greater than zero");
            }

            if (options.RetryCount <= 0)
            {
                return ValidateOptionsResult.Fail($"The {nameof(options.RetryCount)} must be greater than zero");
            }

            if (options.RetryTimeout <= TimeSpan.Zero)
            {
                return ValidateOptionsResult.Fail($"The {nameof(options.RetryTimeout)} must be greater than zero");
            }

            if (options.Timeout <= TimeSpan.Zero)
            {
                return ValidateOptionsResult.Fail($"The {nameof(options.Timeout)} must be greater than zero");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
