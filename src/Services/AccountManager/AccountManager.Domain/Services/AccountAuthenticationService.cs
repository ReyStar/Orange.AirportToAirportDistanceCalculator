using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AccountManager.Domain.Exceptions;
using AccountManager.Domain.Infrastructure;
using AccountManager.Domain.Interfaces;
using AccountManager.Domain.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AccountManager.Domain.Services
{
    class AccountAuthenticationService: IAccountAuthenticationService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IAccountManagerRepository _accountManagerRepository;
        private readonly AccountManagerConfig _accountManagerConfig;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AccountAuthenticationService(IOptions<AccountManagerConfig> accountManagerConfig,
                                            ITokenRepository tokenRepository,
                                            IAccountManagerRepository accountManagerRepository,
                                            IMapper mapper,
                                            ILogger<AccountAuthenticationService> logger)
        {
            _accountManagerConfig = accountManagerConfig.Value;
            _tokenRepository = tokenRepository;
            _accountManagerRepository = accountManagerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AuthenticateModel> LoginAsync(string username, string password,
                                                        CancellationToken cancellationToken)
        {
            var accountInfo = await _accountManagerRepository.GetByUserNameAsync(username, cancellationToken);
            if (accountInfo == null)
            {
                throw new AccountAuthenticationException("Account not found");
            }

            VerifyPasswordHash(password, accountInfo.PasswordHash, accountInfo.PasswordSalt);

            return await LoginAsync(accountInfo, cancellationToken);
        }

        public async Task<AuthenticateModel> RefreshTokenAsync(Guid accountId, string refreshToken, 
                                                               CancellationToken cancellationToken)
        {
            var accountInfo = await _accountManagerRepository.GetByIdAsync(accountId, cancellationToken);
            if (accountInfo == null)
            {
                throw new AccountAuthenticationException("Account not found");
            }

            var refreshTokens = await _tokenRepository.GetAllRefreshTokensAsync(accountId, cancellationToken);

            var refreshTokenModel = refreshTokens.SingleOrDefault(x => x.Token == refreshToken);

            if (refreshTokenModel == null || refreshTokenModel.IsExpired)
            {
                throw new AccountAuthenticationException("The refresh token has expired");
            }

            return await LoginAsync(accountInfo, cancellationToken);
        }

        public async Task LogoutAsync(Guid accountId, CancellationToken cancellationToken)
        { 
            await _tokenRepository.DeleteRefreshTokenAsync(accountId, cancellationToken);
            await _tokenRepository.DeleteAccessTokenAsync(accountId, cancellationToken);
        }

        private async Task<AuthenticateModel> LoginAsync(AccountInfo accountInfo, CancellationToken cancellationToken)
        {
            await LogoutAsync(accountInfo.Id, cancellationToken);

            var currentTime = DateTime.UtcNow;
            var accessToken = GenerateAccessToken(currentTime, accountInfo.Id);
            var refreshToken = GenerateRefreshToken(currentTime, accountInfo.Id);

            await _tokenRepository.AddRefreshTokenAsync(refreshToken, cancellationToken);
            await _tokenRepository.AddAccessTokenAsync(accessToken, cancellationToken);

            var authenticateModel = _mapper.Map<AuthenticateModel>(accountInfo);
            authenticateModel.AccessToken = accessToken.Token;
            authenticateModel.RefreshToken = refreshToken.Token;
            authenticateModel.ExpiresIn = (int)(accessToken.Expires - DateTime.UtcNow).TotalSeconds;

            return authenticateModel;
        }

        private AccessToken GenerateAccessToken(DateTime currentTime, Guid accountId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_accountManagerConfig.SecretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, accountId.ToString())
                }),
                NotBefore = currentTime,
                Expires = currentTime.Add(_accountManagerConfig.AccessTokenExpires),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), _accountManagerConfig.Algorithm)
            };

            var token = (JwtSecurityToken)tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var accessTokenModel = new AccessToken
            {
                AccountId = accountId,
                Token = tokenString,
                TokenHash = token.RawSignature,
                Created = currentTime,
                Expires = currentTime.Add(_accountManagerConfig.AccessTokenExpires)
            };

            return accessTokenModel;
        }

        private RefreshToken GenerateRefreshToken(DateTime currentTime, Guid accountId)
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            var refreshToken = Convert.ToBase64String(randomBytes);

            var refreshTokenModel = new RefreshToken
            {
                AccountId = accountId,
                Token = refreshToken,
                Expires = currentTime.Add(_accountManagerConfig.RefreshTokenExpires),
                Created = currentTime
            };

            return refreshTokenModel;
        }

        private void VerifyPasswordHash(string password, byte[] passwordHash, byte[] storedSalt)
        {
            if (passwordHash.Length != 64)
            {
                _logger.LogError($"Invalid length of {nameof(passwordHash)} (64 bytes)");

                throw new AccountAuthenticationException("Verify password exception");
            }

            if (storedSalt.Length != 128)
            {
                _logger.LogError($"Invalid length of {nameof(passwordHash)} (128 bytes)");

                throw new AccountAuthenticationException("Verify password exception");
            }

            using var hmac = new HMACSHA512(storedSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            if (!computedHash.SequenceEqual(passwordHash))
            {
                throw new AccountAuthenticationException("Verify password exception");
            }
        }
    }
}
