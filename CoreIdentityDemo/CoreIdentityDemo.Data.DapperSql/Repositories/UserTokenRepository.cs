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
    internal class UserTokenRepository : BaseRepository, IUserTokenRepository
    {
        public UserTokenRepository(IDbTransaction transaction)
            : base(transaction)
        { }

        public void Add(UserToken entity)
        {
            AddAsync(entity).Wait();
        }

        public async Task AddAsync(UserToken entity)
        {
            await Connection.ExecuteAsync
            (
                sql:
                @"
                    INSERT INTO [UserTokens]([UserId], [LoginProvider], [Name], [Value])
                    VALUES(@UserId, @LoginProvider, @Name, @Value)
                ",
                param: new
                {
                    UserId = entity.UserId,
                    LoginProvider = entity.LoginProvider,
                    Name = entity.Name,
                    Value = entity.Value
                },
                transaction: Transaction
            );
        }

        public void Delete(UserToken entity)
        {
            DeleteAsync(entity).Wait();
        }

        public async Task DeleteAsync(UserToken entity)
        {
            await Connection.ExecuteAsync
            (
                sql:
                @"
                    DELETE FROM
                        [UserTokens]
                    WHERE
                        [UserId] = @UserId
                        AND [LoginProvider] = @LoginProvider
                        AND [Name] = @Name
                ",
                param: new
                {
                    UserId = entity.UserId,
                    LoginProvider = entity.LoginProvider,
                    Name = entity.Name
                },
                transaction: Transaction
            );
        }

        public IEnumerable<UserToken> GetAll()
        {
            return GetAllAsync().Result;
        }

        public async Task<IEnumerable<UserToken>> GetAllAsync()
        {
            return await Connection.QueryAsync<UserToken>(
                sql:
                @"
                    SELECT
                        [UserId],
                        [LoginProvider],
                        [Name],
                        [Value]
                    FROM
                        [UserTokens]
                ",
                transaction: Transaction
            );
        }

        public UserToken GetByKey(UserTokenKey key)
        {
            return GetByKeyAsync(key).Result;
        }

        public async Task<UserToken> GetByKeyAsync(UserTokenKey key)
        {
            return await Connection.QuerySingleOrDefaultAsync<UserToken>(
                sql:
                @"
                    SELECT
                        [UserId],
                        [LoginProvider],
                        [Name],
                        [Value]
                    FROM
                        [UserTokens]
                    WHERE
                        [UserId] = @UserId
                        AND [LoginProvider] = @LoginProvider
                        AND [Name] = @Name
                ",
                param: new
                {
                    UserId = key.UserId,
                    LoginProvider = key.LoginProvider,
                    Name = key.Name
                },
                transaction: Transaction
            );
        }

        public IEnumerable<UserToken> GetByUserId(Guid userId)
        {
            return GetByUserIdAsync(userId).Result;
        }

        public async Task<IEnumerable<UserToken>> GetByUserIdAsync(Guid userId)
        {
            return await Connection.QueryAsync<UserToken>(
                sql:
                @"
                    SELECT
                        [UserId],
                        [LoginProvider],
                        [Name],
                        [Value]
                    FROM
                        [UserTokens]
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

        public void Update(UserToken entity)
        {
            UpdateAsync(entity).Wait(0);
        }

        public async Task UpdateAsync(UserToken entity)
        {
            await Connection.ExecuteAsync
            (
                sql:
                @"
                    UPDATE
                        [UserTokens]
                    SET
                        [Value] = @Value
                    WHERE
                        [UserId] = @UserId
                        AND [LoginProvider] = @LoginProvider
                        AND [Name] = @Name
                ",
                param: new
                {
                    UserId = entity.UserId,
                    LoginProvider = entity.LoginProvider,
                    Name = entity.Name
                },
                transaction: Transaction
            );
            throw new NotImplementedException();
        }
    }
}
