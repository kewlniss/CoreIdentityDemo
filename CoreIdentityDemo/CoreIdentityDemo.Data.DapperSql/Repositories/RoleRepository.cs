using CoreIdentityDemo.Domain.Entities;
using CoreIdentityDemo.Domain.Repositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace CoreIdentityDemo.Data.DapperSql.Repositories
{
    internal class RoleRepository : BaseRepository, IRoleRepository
    {
        public RoleRepository(IDbTransaction transaction)
            : base(transaction)
        { }

        public void Add(Role entity)
        {
            AddAsync(entity).Wait();
        }

        public async Task AddAsync(Role entity)
        {
            var sql = @"
                INSERT INTO
                    [Role](
                        [Id],
                        [Name],
                        [NormalizedName],
                        [ConcurrencyStamp])
                    VALUES(
                        @Id,
                        @Name,
                        @NormalizedName,
                        @ConcurrencyStamp)
            ";

            var param = new
            {
                Id = entity.Id,
                Name = entity.Name,
                NormalizedName = entity.NormalizedName,
                ConcurrencyStamp = entity.ConcurrencyStamp
            };

            await Connection.ExecuteAsync
            (
                sql: sql,
                param: param,
                transaction: Transaction
            );
        }

        public void Delete(Role entity)
        {
            DeleteAsync(entity).Wait();
        }

        public async Task DeleteAsync(Role entity)
        {
            await Connection.ExecuteAsync
            (
                sql:
                @"
                    DELETE FROM
                        [Role]
                    WHERE
                        [Id] = @Id
                ",
                param: new { Id = entity.Id },
                transaction: Transaction
            );
        }

        public IEnumerable<Role> GetAll()
        {
            return GetAllAsync().Result;
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await Connection.QueryAsync<Role>
            (
                sql:
                @"
                    SELECT
                        [Id],
                        [Name],
                        [NormalizedName],
                        [ConcurrencyStamp]
                    FROM
                        [Role]
                ",
                transaction: Transaction
            );

            throw new NotImplementedException();
        }

        public Role GetByKey(Guid key)
        {
            return GetByKeyAsync(key).Result;
        }

        public async Task<Role> GetByKeyAsync(Guid key)
        {
            return await Connection.QuerySingleOrDefaultAsync<Role>
            (
                sql:
                @"
                    SELECT
                        [Id],
                        [Name],
                        [NormalizedName],
                        [ConcurrencyStamp]
                    FROM
                        [Role]
                    WHERE
                        [Id] = @Id
                ",
                param: new { Id = key },
                transaction: Transaction
            );
        }

        public Role GetByNormalizedName(string normalizedName)
        {
            return GetByNormalizedNameAsync(normalizedName).Result;
        }

        public async Task<Role> GetByNormalizedNameAsync(string normalizedName)
        {
            return await Connection.QuerySingleOrDefaultAsync<Role>
            (
                sql:
                @"
                    SELECT
                        [Id],
                        [Name],
                        [NormalizedName],
                        [ConcurrencyStamp]
                    FROM
                        [Role]
                    WHERE
                        [NormalizedName] = @NormalizedName
                ",
                param: new { NormalizedName = normalizedName },
                transaction: Transaction
            );
        }

        public IEnumerable<Role> GetByUserId(Guid userId)
        {
            return GetByUserIdAsync(userId).Result;
        }

        public async Task<IEnumerable<Role>> GetByUserIdAsync(Guid userId)
        {
            return await Connection.QueryAsync<Role>
            (
                sql:
                @"
                    SELECT
                        r.[Id],
                        r.[Name],
                        r.[Normalizedname],
                        r.[ConcurrencyStamp]
                    FROM
                        [UserRoles] INNER JOIN
                        [Roles] r ON ur.RoleId = r.Id
                    WHERE
                        ur.UserId = @UserId
                ",
                param: new { UserId = userId },
                transaction: Transaction
            );
        }

        public void Update(Role entity)
        {
            UpdateAsync(entity).Wait();
        }

        public async Task UpdateAsync(Role entity)
        {
            await Connection.ExecuteAsync
            (
                sql:
                @"
                    UPDATE
                        [Role]
                    SET
                        [Name] = @Name,
                        [NormalizedName] = @NormalizedName,
                        [ConcurrencyStamp] = @ConcurrencyStamp
                    WHERE
                        [Id] = @Id
                ",
                param: new
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    NormalizedName = entity.NormalizedName,
                    ConcurrencyStamp = entity.ConcurrencyStamp
                },
                transaction: Transaction
            );
        }
    }
}
