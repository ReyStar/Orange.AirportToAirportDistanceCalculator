using Microsoft.Extensions.Options;

namespace AirportInfo.Repository.Infrastructure
{
    /// <summary>
    /// Data base connection strings configuration validator
    /// </summary>
    class ConnectionStringsValidator : IValidateOptions<ConnectionStrings>
    {
        public ValidateOptionsResult Validate(string name, ConnectionStrings options)
        {
            if (options.RequiredVersion <= 0)
            {
                return ValidateOptionsResult.Fail($"The {nameof(options.RequiredVersion)} must be greater than zero");
            }

            if (string.IsNullOrWhiteSpace(options.DefaultConnection))
            {
                return ValidateOptionsResult.Fail($"The {nameof(options.DefaultConnection)} cannot be null or empty");
            }

            if (string.IsNullOrWhiteSpace(options.DataBase))
            {
                return ValidateOptionsResult.Fail($"The {nameof(options.DataBase)} cannot be null or empty");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
