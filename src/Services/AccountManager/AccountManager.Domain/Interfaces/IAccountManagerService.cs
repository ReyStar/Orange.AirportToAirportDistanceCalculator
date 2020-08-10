using System;
using System.Threading;
using System.Threading.Tasks;
using AccountManager.Domain.Models;

namespace AccountManager.Domain.Interfaces
{
    /// <summary>
    /// Service for working with user accounts
    /// </summary>
    public interface IAccountManagerService
    {
        Task<Account> GetByIdAsync(Guid accountId, CancellationToken cancellationToken = default);

        Task<Account> CreateAsync(RegisterAccountModel registerAccountModel, CancellationToken cancellationToken = default);

        Task<Account> UpdateAsync(UpdateAccountModel updateAccountModel, CancellationToken cancellationToken = default);

        Task DeleteAsync(Guid accountId, CancellationToken cancellationToken = default);
    }
}
