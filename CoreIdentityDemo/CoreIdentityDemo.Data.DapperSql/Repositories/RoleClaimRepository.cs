using CoreIdentityDemo.Domain.Entities;
using CoreIdentityDemo.Domain.Repositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CoreIdentityDemo.Data.DapperSql.Repositories
{
    internal class RoleClaimRepository : BaseRepository, IRoleClaimRepository
    {
        public RoleClaimRepository(IDbTransaction transaction)
            : base(transaction)
        { }

        public void Add(RoleClaim entity)
        {
            AddAsync(entity).Wait();
        }

        public async Task AddAsync(RoleClaim entity)
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
                    INSERT INTO [RoleClaims](
                        [Id],
                        [RoleId])
                    VALUES(
                        @Id,
                        @RoleId)
                ",
                param: new
                {
                    Id = id,
                    RoleId = entity.RoleId
                }
            );
        }

        public void Delete(RoleClaim entity)
        {
            DeleteAsync(entity).Wait();
        }

        public async Task DeleteAsync(RoleClaim entity)
        {
            await Connection.ExecuteAsync
            (
                sql:
                @"
                    DELETE FROM
                        [RoleClaims]
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

        public IEnumerable<RoleClaim> GetAll()
        {
            return GetAllAsync().Result;
        }

        public async Task<IEnumerable<RoleClaim>> GetAllAsync()
        {
            return await Connection.QueryAsync<RoleClaim>
            (
                sql:
                @"
                    SELECT
                        c.[Id],
                        c.[Type],
                        c.[Value],
                        rc.[RoleId]
                    FROM
                        [Claims] c INNER JOIN
                        [RoleClaims] rc ON c.[Id] = rc.[Id]
                ",
                transaction: Transaction
            );
        }

        public RoleClaim GetByKey(int key)
        {
            return GetByKeyAsync(key).Result;
        }

        public async Task<RoleClaim> GetByKeyAsync(int key)
        {
            return await Connection.QuerySingleOrDefaultAsync<RoleClaim>
            (
                sql:
                @"
                    SELECT
                        c.[Id],
                        c.[Type],
                        c.[Value],
                        rc.[RoleId]
                    FROM
                        [Claims] c INNER JOIN
                        [RoleClaims] rc ON c.[Id] = rc.[Id]
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

        public IEnumerable<RoleClaim> GetByRoleId(Guid roleId)
        {
            return GetByRoleIdAsync(roleId).Result;
        }

        public async Task<IEnumerable<RoleClaim>> GetByRoleIdAsync(Guid roleId)
        {
            return await Connection.QueryAsync<RoleClaim>
            (
                sql:
                @"
                    SELECT
                        c.[Id],
                        c.[Type],
                        c.[Value],
                        rc.[RoleId]
                    FROM
                        [Claims] c INNER JOIN
                        [RoleClaims] rc ON c.[Id] = rc.[Id]
                    WHERE
                        rc.[RoleId] = @RoleId
                ",
                param: new
                {
                    RoleId = roleId
                },
                transaction: Transaction
            );
        }

        public void Update(RoleClaim entity)
        {
            UpdateAsync(entity).Wait();
        }

        public async Task UpdateAsync(RoleClaim entity)
        {
            await Connection.ExecuteAsync
            (
                sql:
                @"
                    UPDATE
                        [RoleClaims]
                    SET
                        [RoleId] = @RoleId
                    WHERE
                        [Id] = @Id
                ",
                param: new
                {
                    Id = entity.Id,
                    RoleId = entity.RoleId
                },
                transaction: Transaction
            );

            await Connection.ExecuteAsync
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
