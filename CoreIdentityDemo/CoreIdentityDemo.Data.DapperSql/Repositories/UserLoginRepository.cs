using CoreIdentityDemo.Domain.Entities;
using CoreIdentityDemo.Domain.Keys;
using CoreIdentityDemo.Domain.Repositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CoreIdentityDemo.Data.DapperSql.Repositories
{
    internal class UserLoginRepository : BaseRepository, IUserLoginRepository
    {
        public UserLoginRepository(IDbTransaction transaction)
            : base(transaction)
        { }

        public void Add(UserLogin entity)
        {
            AddAsync(entity).Wait();
        }

        public async Task AddAsync(UserLogin entity)
        {
            await Connection.ExecuteAsync
            (
                sql:
                @"
                    INSERT INTO [UserLogins](
                        [LoginProvider],
                        [ProviderKey],
                        [ProviderDisplayName],
                        [UserId])
                    VALUES(
                        @LoginProvider,
                        @ProviderKey,
                        @ProviderDisplayName,
                        @UserId)
                ",
                param: new
                {
                    LoginProvider = entity.LoginProvider,
                    ProviderKey = entity.ProviderKey,
                    ProviderDisplayName = entity.ProviderDisplayName,
                    UserId = entity.UserId
                },
                transaction: Transaction
            );
        }

        public void Delete(UserLogin entity)
        {
            DeleteAsync(entity).Wait();
        }

        public async Task DeleteAsync(UserLogin entity)
        {
            await Connection.ExecuteAsync
            (
                sql:
                @"
                    DELETE FROM
                        [UserLogins]
                    WHERE
                        [LoginProvider] = @LoginProvider
                        AND [ProviderKey] = @ProviderKey
                ",
                param: new
                {
                    LoginProvider = entity.LoginProvider,
                    ProviderKey = entity.ProviderKey
                },
                transaction: Transaction
            );
        }

        public IEnumerable<UserLogin> GetAll()
        {
            return GetAllAsync().Result;
        }

        public async Task<IEnumerable<UserLogin>> GetAllAsync()
        {
            return await Connection.QueryAsync<UserLogin>
            (
                sql:
                @"
                    SELECT
                        [LoginProvider],
                        [ProviderKey],
                        [ProviderDisplayName],
                        [UserId]
                    FROM
                        [UserLogins]
                ",
                transaction: Transaction
            );
        }

        public UserLogin GetByKey(UserLoginKey key)
        {
            return GetByKeyAsync(key).Result;
        }

        public async Task<UserLogin> GetByKeyAsync(UserLoginKey key)
        {
            return await Connection.QuerySingleOrDefaultAsync<UserLogin>
            (
                sql:
                @"
                    SELECT
                        [LoginProvider],
                        [ProviderKey],
                        [ProviderDisplayName],
                        [UserId]
                    FROM
                        [UserLogins]
                    WHERE
                        [LoginProvider] = @LoginProvider,
                        [ProviderKey] = @ProviderKey
                ",
                param: new
                {
                    LoginProvider = key.LoginProvider,
                    ProviderKey = key.ProviderKey
                },
                transaction: Transaction
            );
        }

        public IEnumerable<UserLogin> GetByUserId(Guid userId)
        {
            return GetByUserIdAsync(userId).Result;
        }

        public async Task<IEnumerable<UserLogin>> GetByUserIdAsync(Guid userId)
        {
            return await Connection.QueryAsync<UserLogin>
            (
                sql:
                @"
                    SELECT
                        [LoginProvider],
                        [ProviderKey],
                        [ProviderDisplayName],
                        [UserId]
                    FROM
                        [UserLogins]
                    WHERE
                        [UserId] = @UserId
                ",
                param: new
                {
                    UserId = userId
                },
                transaction: Transaction
            );
        }

        public void Update(UserLogin entity)
        {
            UpdateAsync(entity).Wait();
        }

        public async Task UpdateAsync(UserLogin entity)
        {
            await Connection.ExecuteAsync
            (
                sql:
                @"
                    UPDATE
                        [UserLogins]
                    SET
                        [ProviderDisplayName] = @ProviderDisplayName,
                        [UserId] = @UserId
                    WHERE
                        [LoginProvider] = @LoginProvider
                        AND [ProviderKey] = @ProviderKey
                ",
                param: new
                {
                    LoginProvider = entity.LoginProvider,
                    ProviderKey = entity.ProviderKey,
                    ProviderDisplayName = entity.ProviderDisplayName,
                    UserId = entity.UserId
                },
                transaction: Transaction
            );
        }
    }
}
