using System;
using System.Threading;
using System.Threading.Tasks;
using AccountManager.Domain.Interfaces;
using AccountManager.Domain.Models;
using Microsoft.Extensions.Caching.Memory;

namespace AccountManager.Application.Services
{
    /// <summary>
    /// In memory caching decorator for the AccountManagerService
    /// </summary>
    class AccountAuthenticationMemoryCacheService : IAccountAuthenticationService
    {
        private readonly IAccountAuthenticationService _accountAuthenticationService;
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheEntryOptions _memoryCacheGeoDistanceEntryOptions;

        public AccountAuthenticationMemoryCacheService(IAccountAuthenticationService accountAuthenticationService, IMemoryCache memoryCache)
        {
            _accountAuthenticationService = accountAuthenticationService;
            _memoryCache = memoryCache;

            _memoryCacheGeoDistanceEntryOptions = new MemoryCacheEntryOptions() { Size = 2048 };
        }

        public Task<AuthenticateModel> LoginAsync(string login, string password, CancellationToken cancellationToken = default)
        {
            return _accountAuthenticationService.LoginAsync(login, password, cancellationToken);
        }

        public Task<AuthenticateModel> RefreshTokenAsync(Guid accountId, string refreshToken, CancellationToken cancellationToken = default)
        {
            return _accountAuthenticationService.RefreshTokenAsync(accountId, refreshToken, cancellationToken);
        }

        public Task LogoutAsync(Guid accountId, CancellationToken cancellationToken = default)
        {
            return _accountAuthenticationService.LogoutAsync(accountId, cancellationToken);
        }
    }
}
