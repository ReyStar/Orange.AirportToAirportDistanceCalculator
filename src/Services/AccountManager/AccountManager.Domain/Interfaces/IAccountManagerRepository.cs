using System;
using System.Threading;
using System.Threading.Tasks;
using AccountManager.Domain.Models;

namespace AccountManager.Domain.Interfaces
{
    public interface IAccountManagerRepository
    {
        Task<AccountInfo> GetByUserNameAsync(string username, CancellationToken cancellationToken = default);
        
        Task<AccountInfo> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        
        Task<bool> CreateAccountAsync(AccountInfo accountInfo, CancellationToken cancellationToken);
        
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken);
        
        Task<bool> UpdateAccountAsync(AccountInfo accountInfo, CancellationToken cancellationToken);
    }
}
