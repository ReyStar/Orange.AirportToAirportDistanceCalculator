using System;
using System.Threading;
using System.Threading.Tasks;
using AccountManager.Domain.Models;

namespace AccountManager.Domain.Interfaces
{
    /// <summary>
    /// Service for Authentication control
    /// </summary>
    public interface IAccountAuthenticationService
    {
        Task<AuthenticateModel> LoginAsync(string login, string password, CancellationToken cancellationToken = default);

        Task<AuthenticateModel> RefreshTokenAsync(Guid accountId, string refreshToken, CancellationToken cancellationToken = default);

        Task LogoutAsync(Guid accountId, CancellationToken cancellationToken = default);
    }
}
