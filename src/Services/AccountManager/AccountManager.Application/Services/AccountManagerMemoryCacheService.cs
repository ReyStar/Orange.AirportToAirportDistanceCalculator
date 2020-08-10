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
    class AccountManagerMemoryCacheService : IAccountManagerService
    {
        private readonly IAccountManagerService _accountManagerService;
        private readonly IMemoryCache _memoryCache;
        private readonly MemoryCacheEntryOptions _memoryCacheGeoDistanceEntryOptions;

        public AccountManagerMemoryCacheService(IAccountManagerService accountManagerService, IMemoryCache memoryCache)
        {
            _accountManagerService = accountManagerService;
            _memoryCache = memoryCache;

            _memoryCacheGeoDistanceEntryOptions = new MemoryCacheEntryOptions() { Size = 2048 };
        }

        public Task<Account> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _accountManagerService.GetByIdAsync(id, cancellationToken);
        }

        public Task<Account> CreateAsync(RegisterAccountModel registerAccountModel, CancellationToken cancellationToken = default)
        {
            return _accountManagerService.CreateAsync(registerAccountModel, cancellationToken);
        }

        public Task<Account> UpdateAsync(UpdateAccountModel updateAccountModel, CancellationToken cancellationToken = default)
        {
            return _accountManagerService.UpdateAsync(updateAccountModel, cancellationToken);
        }

        public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return _accountManagerService.DeleteAsync(id, cancellationToken);
        }
    }
}
