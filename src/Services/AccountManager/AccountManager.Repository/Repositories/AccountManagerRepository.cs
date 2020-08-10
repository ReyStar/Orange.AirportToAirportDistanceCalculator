using System;
using System.Linq;
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
    /// <summary>
    /// Repository for storing user accounts information
    /// </summary>
    /// https://blog.maskalik.com/asp-net/sqlite-simple-database-with-dapper/
    /// https://docs.microsoft.com/en-us/dotnet/standard/data/sqlite/dapper-limitations
    internal class AccountManagerRepository : IAccountManagerRepository
    {
        private readonly IDataSource _dataSource;
        private readonly IMapper _mapper;
        private readonly string _accountTableUpdateScript;
        private readonly string _accountTableDeleteScript;
        private readonly string _accountUpdateWithoutPassScript;

        public AccountManagerRepository(IDataSource dataSource, 
                                        IMapper mapper)
        {
            _dataSource = dataSource;
            _mapper = mapper;

            var resourceLoader = new ResourceLoader(typeof(AccountManagerRepository).Assembly);
            _accountTableUpdateScript = resourceLoader.LoadString($"{typeof(AccountManagerRepository).Namespace}.SQL.AccountUpdate.sql");
            _accountUpdateWithoutPassScript = resourceLoader.LoadString($"{typeof(AccountManagerRepository).Namespace}.SQL.AccountUpdateWithoutPass.sql");
            _accountTableDeleteScript = resourceLoader.LoadString($"{typeof(AccountManagerRepository).Namespace}.SQL.AccountDelete.sql");
        }

        public async Task<AccountInfo> GetByUserNameAsync(string username, CancellationToken cancellationToken = default)
        {
            try
            {
                var result =
                    await _dataSource.Connection.FirstOrDefaultAsync<AccountDbModel>(x =>
                        x.Username == username && x.IsDeleted != true);

                return _mapper.Map<AccountInfo>(result);
            }
            catch (Exception ex)
            {
                throw new AccountManagerException("Account getting by user name exception", ex);
            }
        }

        public async Task<AccountInfo> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result =
                    await _dataSource.Connection.FirstOrDefaultAsync<AccountDbModel>(x => 
                        x.Id == id && x.IsDeleted != true);

                return _mapper.Map<AccountInfo>(result);
            }
            catch (Exception ex)
            {
                throw new AccountManagerException("Account getting by id exception", ex);
            }
        }

        public async Task<bool> CreateAccountAsync(AccountInfo accountInfo, CancellationToken cancellationToken)
        {
            var dbModel = _mapper.Map<AccountDbModel>(accountInfo);
            
            try
            {
                var key = await _dataSource.Connection.InsertAsync(dbModel);
                return key != null;
            }
            catch (Exception ex)
            {
                throw new AccountManagerException("Account creation exception", ex);
            }
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var commandDefinition = new CommandDefinition(_accountTableDeleteScript,
                    parameters: new
                    {
                        id = id
                    },
                    cancellationToken: cancellationToken);

                return await _dataSource.Connection.ExecuteAsync(commandDefinition) > 0;
            }
            catch (Exception ex)
            {
                throw new AccountManagerException("Account deleting exception", ex);
            }
        }

        public async Task<bool> UpdateAccountAsync(AccountInfo accountInfo, CancellationToken cancellationToken)
        {
            try
            {
                var dbModel = _mapper.Map<AccountDbModel>(accountInfo);

                var script = dbModel.PasswordHash == null 
                             || !dbModel.PasswordHash.Any()
                             || dbModel.PasswordSalt == null
                             || !dbModel.PasswordSalt.Any()
                    ? _accountUpdateWithoutPassScript
                    : _accountTableUpdateScript;

                var commandDefinition = new CommandDefinition(script,
                    parameters: new
                    {
                        id = dbModel.Id,
                        username = dbModel.Username,
                        firstname = dbModel.FirstName,
                        lastname = dbModel.LastName,
                        password_hash = dbModel.PasswordHash,
                        password_salt = dbModel.PasswordSalt,
                        revision = dbModel.Revision,
                        is_deleted = dbModel.IsDeleted
                    },
                    cancellationToken: cancellationToken);

                return await _dataSource.Connection.ExecuteAsync(commandDefinition) > 0;
            }
            catch (Exception ex)
            {
                throw new AccountManagerException("Account update exception", ex);
            }
        }
    }
}
