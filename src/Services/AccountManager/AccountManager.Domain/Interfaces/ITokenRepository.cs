using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccountManager.Domain.Models;

namespace AccountManager.Domain.Interfaces
{
    public interface ITokenRepository
    {
        Task<bool> AddRefreshTokenAsync(RefreshToken refreshTokenModel, CancellationToken cancellationToken);
        
        Task<IEnumerable<RefreshToken>> GetAllRefreshTokensAsync(Guid accountId, CancellationToken cancellationToken);

        Task<bool> DeleteAccessTokenAsync(Guid accountId, CancellationToken cancellationToken);

        Task<bool> DeleteRefreshTokenAsync(Guid accountId, CancellationToken cancellationToken);

        Task<bool> AddAccessTokenAsync(AccessToken accessToken, CancellationToken cancellationToken);
        
        Task<IEnumerable<AccessToken>> GetAllAccessTokensAsync(Guid accountInfoId, CancellationToken cancellationToken);
    }
}
