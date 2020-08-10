using Microsoft.Extensions.Options;

namespace AccountManager.Domain.Infrastructure
{
    class AccountManagerConfigValidator : IValidateOptions<AccountManagerConfig>
    {
        public ValidateOptionsResult Validate(string name, AccountManagerConfig options)
        {
            if (string.IsNullOrWhiteSpace(options.Algorithm))
            {
                return ValidateOptionsResult.Fail($"The {nameof(options.Algorithm)} must be specified");
            }

            if (string.IsNullOrWhiteSpace(options.SecretKey))
            {
                return ValidateOptionsResult.Fail($"The {nameof(options.SecretKey)} must be specified");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
