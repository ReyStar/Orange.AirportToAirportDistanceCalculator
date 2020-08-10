using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccountManager.Common;
using AccountManager.Domain.Exceptions;
using AccountManager.Domain.Interfaces;
using AccountManager.Domain.Models;
using AccountManager.Repository.Models;
using AutoMapper;
using Dapper;
using Dapper.Dommel;

namespace AccountManager.Repository.Repositories
{
    class TokenRepository: ITokenRepository
    {
        private readonly IDataSource _dataSource;
        private readonly IMapper _mapper;
        private static readonly string AccessTokenDeleteScript;
        private static readonly string RefreshTokenDeleteScript;


        static TokenRepository()
        {
            var resourceLoader = new ResourceLoader(typeof(TokenRepository).Assembly);
            
            AccessTokenDeleteScript = resourceLoader.LoadString($"{typeof(AccountManagerRepository).Namespace}.SQL.AccessTokenDelete.sql");
            RefreshTokenDeleteScript = resourceLoader.LoadString($"{typeof(AccountManagerRepository).Namespace}.SQL.RefreshTokenDelete.sql");
        }

        public TokenRepository(IDataSource dataSource,
                               IMapper mapper)
        {
            _dataSource = dataSource;
            _mapper = mapper;
        }

        public async Task<bool> AddRefreshTokenAsync(RefreshToken refreshTokenModel, CancellationToken cancellationToken)
        {
            var dbModel = _mapper.Map<RefreshTokenDbModel>(refreshTokenModel);

            try
            {
                var key = await _dataSource.Connection.InsertAsync(dbModel);

                return key != null;
            }
            catch (Exception ex)
            {
                throw new AccountManagerException("Refresh token creation exception", ex);
            }
        }

        public async Task<bool> AddAccessTokenAsync(AccessToken accessToken, CancellationToken cancellationToken)
        {
            var dbModel = _mapper.Map<AccessTokenDbModel>(accessToken);
            dbModel.Token = null;
            try
            {
                var key = await _dataSource.Connection.InsertAsync(dbModel);

                return key != null;
            }
            catch (Exception ex)
            {
                throw new AccountManagerException("Access token creation exception", ex);
            }
        }

        public async Task<IEnumerable<AccessToken>> GetAllAccessTokensAsync(Guid accountId, CancellationToken cancellationToken)
        {
            try
            {
                var refreshTokens = await _dataSource.Connection.SelectAsync<AccessTokenDbModel>(x => !x.IsDeleted && x.AccountId == accountId);

                return _mapper.Map<IEnumerable<AccessToken>>(refreshTokens);
            }
            catch (Exception ex)
            {
                throw new AccountManagerException("Get access tokens exception", ex);
            }
        }

        public async Task<IEnumerable<RefreshToken>> GetAllRefreshTokensAsync(Guid accountId, CancellationToken cancellationToken)
        {
            try
            {
                var refreshTokens = await _dataSource.Connection.SelectAsync<RefreshTokenDbModel>(x => !x.IsDeleted && x.AccountId == accountId);

                return _mapper.Map<IEnumerable<RefreshToken>>(refreshTokens);
            }
            catch (Exception ex)
            {
                throw new AccountManagerException("Get refresh tokens exception", ex);
            }
        }

        public async Task<bool> DeleteAccessTokenAsync(Guid accountId, CancellationToken cancellationToken)
        {
            try
            {
                var commandDefinition = new CommandDefinition(AccessTokenDeleteScript,
                    parameters: new
                    {
                        accountId = accountId
                    },
                    cancellationToken: cancellationToken);

                return await _dataSource.Connection.ExecuteAsync(commandDefinition) > 0;
            }
            catch (Exception ex)
            {
                throw new AccountManagerException("Access token deleting exception", ex);
            }
        }

        public async Task<bool> DeleteRefreshTokenAsync(Guid accountId, CancellationToken cancellationToken)
        {
            try
            {
                var commandDefinition = new CommandDefinition(RefreshTokenDeleteScript,
                    parameters: new
                    {
                        accountId = accountId
                    },
                    cancellationToken: cancellationToken);

                return await _dataSource.Connection.ExecuteAsync(commandDefinition) > 0;
            }
            catch (Exception ex)
            {
                throw new AccountManagerException("Refresh token deleting exception", ex);
            }
        }
    }
}
