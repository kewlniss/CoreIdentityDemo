using CoreIdentityDemo.Domain.Entities;
using CoreIdentityDemo.Domain.Repositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CoreIdentityDemo.Data.DapperSql.Repositories
{
    internal class UserClaimRepository : BaseRepository, IUserClaimRepository
    {
        public UserClaimRepository(IDbTransaction transaction)
            : base(transaction)
        { }

        public void Add(UserClaim entity)
        {
            AddAsync(entity).Wait();
        }

        public async Task AddAsync(UserClaim entity)
        {
            var id = await Connection.ExecuteScalarAsync<int>
            (
                sql:
                @"
                    INSERT INTO [Claims](
                        [Type],
                        [Value])
                    VALUES(
                        @Type,
                        @Value);
                    SELECT SCOPE_IDENTITY()
                ",
                param: new
                {
                    Type = entity.Type,
                    Value = entity.Value
                },
                transaction: Transaction
            );

            await Connection.ExecuteAsync
            (
                sql:
                @"
                    INSERT INTO [UserClaims](
                        [Id],
                        [UserId])
                    VALUES(
                        @Id,
                        @UserId)
                ",
                param: new
                {
                    Id = id,
                    RoleId = entity.UserId
                }
            );
        }

        public void Delete(UserClaim entity)
        {
            DeleteAsync(entity).Wait();
        }

        public async Task DeleteAsync(UserClaim entity)
        {
            await Connection.ExecuteAsync
            (
                sql:
                @"
                    DELETE FROM
                        [UserClaims]
                    WHERE
                        Id = @Id
                ",
                param: new
                {
                    Id = entity.Id
                },
                transaction: Transaction
            );

            await Connection.ExecuteAsync
            (
                sql:
                @"
                    DELETE FROM
                        [Claims]
                    WHERE
                        Id = @Id
                ",
                param: new
                {
                    Id = entity.Id
                },
                transaction: Transaction
            );
        }

        public IEnumerable<UserClaim> GetAll()
        {
            return GetAllAsync().Result;
        }

        public async Task<IEnumerable<UserClaim>> GetAllAsync()
        {
            return await Connection.QueryAsync<UserClaim>
            (
                sql:
                @"
                    SELECT
                        c.[Id],
                        c.[Type],
                        c.[Value],
                        uc.[UserId]
                    FROM
                        [Claims] c INNER JOIN
                        [UserClaims] uc ON c.[Id] = uc.[Id]
                ",
                transaction: Transaction
            );
        }

        public UserClaim GetByKey(int key)
        {
            return GetByKeyAsync(key).Result;
        }

        public async Task<UserClaim> GetByKeyAsync(int key)
        {
            return await Connection.QueryFirstAsync<UserClaim>
            (
                sql:
                @"
                    SELECT
                        c.[Id],
                        c.[Type],
                        c.[Value],
                        uc.[UserId]
                    FROM
                        [Claims] c INNER JOIN
                        [UserClaims] uc ON c.[Id] = uc.[Id]
                    WHERE
                        c.[Id] = @Id
                ",
                param: new
                {
                    Id = key
                },
                transaction: Transaction
            );
        }

        public IEnumerable<UserClaim> GetByUserId(Guid userId)
        {
            return GetByUserIdAsync(userId).Result;
        }

        public async Task<IEnumerable<UserClaim>> GetByUserIdAsync(Guid userId)
        {
            return await Connection.QueryAsync<UserClaim>
            (
                sql:
                @"
                    SELECT
                        c.[Id],
                        c.[Type],
                        c.[Value],
                        uc.[UserId]
                    FROM
                        [Claims] c INNER JOIN
                        [UserClaims] uc ON c.[Id] = uc.[Id]
                    WHERE
                        uc.[UserId] = @UserId
                ",
                param: new
                {
                    UserId = userId
                },
                transaction: Transaction
            );
        }

        public void Update(UserClaim entity)
        {
            UpdateAsync(entity).Wait();
        }

        public async Task UpdateAsync(UserClaim entity)
        {
            await Connection.ExecuteAsync
            (
                sql:
                @"
                    UPDATE
                        [UserClaims]
                    SET
                        [UserId] = @UserId
                    WHERE
                        [Id] = @Id
                ",
                param: new
                {
                    Id = entity.Id,
                    UserId = entity.UserId
                },
                transaction: Transaction
            );

            Connection.Execute
            (
                sql:
                @"
                    UPDATE
                        [Claims]
                    SET
                        [Type] = @Type
                        [Value] = @Value
                    WHERE
                        [Id] = @Id
                ",
                param: new
                {
                    Id = entity.Id,
                    Type = entity.Type,
                    Value = entity.Value
                }
            );
        }
    }
}
