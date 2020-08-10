using System;
using System.Collections;
using System.Collections.Generic;
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
    class AccountManagerService : IAccountManagerService
    {
        private readonly AccountManagerConfig _accountManagerConfig;
        private readonly IAccountManagerRepository _accountManagerRepository;
        private readonly IAccountAuthenticationService _accountAuthenticationService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AccountManagerService(IOptions<AccountManagerConfig> accountManagerConfig,
                                     IAccountManagerRepository accountManagerRepository,
                                     IAccountAuthenticationService accountAuthenticationService,
                                     IMapper mapper,
                                     ILogger<AccountManagerService> logger)
        {
            _accountManagerConfig = accountManagerConfig.Value;
            _accountManagerRepository = accountManagerRepository;
            _accountAuthenticationService = accountAuthenticationService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Account> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var accountInfo = await _accountManagerRepository.GetByIdAsync(id, cancellationToken);

            return _mapper.Map<Account>(accountInfo);
        }

        public async Task<Account> CreateAsync(RegisterAccountModel registerAccountModel,
                                               CancellationToken cancellationToken = default)
        {
            //var existAccount = await _accountManagerRepository.GetByUserNameAsync(registerAccountModel.Username, cancellationToken);
            //if (existAccount != null)
            //{
            //    throw new AccountManagerException($"User {registerAccountModel.Username} already exist");
            //}

            var hash = CreatePasswordHash(registerAccountModel.Password);
            
            var accountInfo = _mapper.Map<AccountInfo>(registerAccountModel);
            
            accountInfo.Id = Guid.NewGuid();
            accountInfo.PasswordHash = hash.passwordHash;
            accountInfo.PasswordSalt = hash.passwordSalt;

            var result = await _accountManagerRepository.CreateAccountAsync(accountInfo, cancellationToken);

            //if (!result)
            //{
            //    throw new AccountManagerException("The user updateAccountModel was not created");
            //}

            return accountInfo;
        }

        public async Task<Account> UpdateAsync(UpdateAccountModel updateAccountModel, 
                                               CancellationToken cancellationToken = default)
        {
            var accountInfo = _mapper.Map<AccountInfo>(updateAccountModel);

            if (!string.IsNullOrWhiteSpace(updateAccountModel.Password))
            {
                var hash = CreatePasswordHash(updateAccountModel.Password);
                accountInfo.PasswordHash = hash.passwordHash;
                accountInfo.PasswordSalt = hash.passwordSalt;
            }

            var result = await _accountManagerRepository.UpdateAccountAsync(accountInfo, cancellationToken);

            if (!result)
            {
                throw new AccountManagerException("User update error");
            }

            return accountInfo;
        }

        public async Task DeleteAsync(Guid accountId, CancellationToken cancellationToken = default)
        {
            await _accountAuthenticationService.LogoutAsync(accountId, cancellationToken);

            var result = await _accountManagerRepository.DeleteAsync(accountId, cancellationToken);

            if (!result)
            {
                throw new AccountManagerException("The account was not found");
            }
        }

        private (byte[] passwordSalt, byte[] passwordHash) CreatePasswordHash(string password)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
            }

            using var hmac = new HMACSHA512();

            return (passwordSalt: hmac.Key, passwordHash: hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }
}
