using CoreIdentityDemo.Domain.Entities;
using CoreIdentityDemo.Domain.Keys;
using CoreIdentityDemo.Domain.Repositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Linq;

namespace CoreIdentityDemo.Data.DapperSql.Repositories
{
    internal class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(IDbTransaction transaction)
            : base(transaction)
        { }

        public async Task AddAsync(User entity)
        {
            await Connection.ExecuteAsync
            (
                sql:
                @"
                    INSERT INTO [Users](
                        [Id],
                        [UserName],
                        [NormalizedUserName],
                        [Email],
                        [NormalizedEmail],
                        [EmailConfirmed],
                        [PasswordHash],
                        [SecurityStamp],
                        [ConcurrencyStamp],
                        [PhoneNumber],
                        [PhoneNumberConfirmed],
                        [TwoFactorEnabled],
                        [LockoutEnd],
                        [LockoutEnabled],
                        [AccessFailedCount])
                    VALUES (
                        @Id,
                        @UserName,
                        @NormalizedUserName,
                        @Email,
                        @NormalizedEmail,
                        @EmailConfirmed,
                        @PasswordHash,
                        @SecurityStamp,
                        @ConcurrencyStamp,
                        @PhoneNumber,
                        @PhoneNumberConfirmed,
                        @TwoFactorEnabled,
                        @LockoutEnd,
                        @LockoutEnabled,
                        @AccessFailedCount)
                ",
                param: new
                {
                    Id = entity.Id,
                    UserName = entity.UserName,
                    NormalizeduserName = entity.NormalizedUserName,
                    Email = entity.Email,
                    NormalizedEmail = entity.NormalizedEmail,
                    EmailConfirmed = entity.EmailConfirmed,
                    PasswordHash = entity.PasswordHash,
                    SecurityStamp = entity.SecurityStamp,
                    ConcurrencyStamp = entity.ConcurrencyStamp,
                    PhoneNumber = entity.PhoneNumber,
                    PhoneNumberConfirmed = entity.PhoneNumberConfirmed,
                    TwoFactorEnabled = entity.TwoFactorEnabled,
                    LockoutEnd = entity.LockoutEnd,
                    LockoutEnabled = entity.LockoutEnabled,
                    AccessFailedCount = entity.AccessFailedCount
                },
                transaction: Transaction
            );
        }

        public async Task DeleteAsync(User entity)
        {
            await Connection.ExecuteAsync
            (
                sql:
                @"
                    DELETE FROM
                        [Users]
                    WHERE
                        [Id] = @Id
                ",
                param: new
                {
                    Id = entity.Id
                },
                transaction: Transaction
            );
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await Connection.QueryAsync<User>
            (
                sql:
                @"
                    SELECT
                        [Id],
                        [UserName],
                        [NormalizeduserName],
                        [Email],
                        [NormalizedEmail],
                        [EmailConfirmed],
                        [PasswordHash],
                        [SecurityStamp],
                        [ConcurrencyStamp],
                        [PhoneNumber],
                        [PhoneNumberConfirmed],
                        [TwoFactorEnabled],
                        [LockoutEnd],
                        [LockoutEnabled],
                        [AccessFailedCount]
                    FROM
                        [Users]
                ",
                transaction: Transaction
            );
        }

        public async Task<User> GetByKeyAsync(Guid key)
        {
            return await Connection.QuerySingleOrDefaultAsync<User>
            (
                sql:
                @"
                    SELECT
                        [Id],
                        [UserName],
                        [NormalizeduserName],
                        [Email],
                        [NormalizedEmail],
                        [EmailConfirmed],
                        [PasswordHash],
                        [SecurityStamp],
                        [ConcurrencyStamp],
                        [PhoneNumber],
                        [PhoneNumberConfirmed],
                        [TwoFactorEnabled],
                        [LockoutEnd],
                        [LockoutEnabled],
                        [AccessFailedCount]
                    FROM
                        [Users]
                    WHERE
                        [Id] = @Id
                ",
                param: new
                {
                    Id = key
                },
                transaction: Transaction
            );
        }

        public async Task<User> GetByNormalizedEmailAsync(string normalizedEmail)
        {
            return await Connection.QuerySingleOrDefaultAsync<User>
            (
                sql:
                @"
                    SELECT
                        [Id],
                        [UserName],
                        [NormalizeduserName],
                        [Email],
                        [NormalizedEmail],
                        [EmailConfirmed],
                        [PasswordHash],
                        [SecurityStamp],
                        [ConcurrencyStamp],
                        [PhoneNumber],
                        [PhoneNumberConfirmed],
                        [TwoFactorEnabled],
                        [LockoutEnd],
                        [LockoutEnabled],
                        [AccessFailedCount]
                    FROM
                        [Users]
                    WHERE
                        [NormalizedEmail] = @NormalizedEmail
                ",
                param: new
                {
                    NormalizedEmail = normalizedEmail
                },
                transaction: Transaction
            );
        }

        public async Task<User> GetByNormalizedUsernameAsync(string normalizedUsername)
        {
            return await Connection.QuerySingleOrDefaultAsync<User>
            (
                sql:
                @"
                    SELECT
                        [Id],
                        [UserName],
                        [NormalizeduserName],
                        [Email],
                        [NormalizedEmail],
                        [EmailConfirmed],
                        [PasswordHash],
                        [SecurityStamp],
                        [ConcurrencyStamp],
                        [PhoneNumber],
                        [PhoneNumberConfirmed],
                        [TwoFactorEnabled],
                        [LockoutEnd],
                        [LockoutEnabled],
                        [AccessFailedCount]
                    FROM
                        [Users]
                    WHERE
                        [NormalizedUserName] = @NormalizedUserName
                ",
                param: new
                {
                    NormalizedUserName = normalizedUsername
                },
                transaction: Transaction
            );
        }

        public async Task<User> GetByUserLoginAsync(UserLoginKey userLoginKey)
        {
            return await Connection.QuerySingleOrDefaultAsync<User>
            (
                sql:
                @"
                    SELECT
                        u.[Id],
                        u.[UserName],
                        u.[NormalizeduserName],
                        u.[Email],
                        u.[NormalizedEmail],
                        u.[EmailConfirmed],
                        u.[PasswordHash],
                        u.[SecurityStamp],
                        u.[ConcurrencyStamp],
                        u.[PhoneNumber],
                        u.[PhoneNumberConfirmed],
                        u.[TwoFactorEnabled],
                        u.[LockoutEnd],
                        u.[LockoutEnabled],
                        u.[AccessFailedCount]
                    FROM
                        [UserLogins] ul INNER JOIN
                        [Users] u ON ul.[UserId] = u.[Id]
                    WHERE
                        ul.[LoginProvider] = @LoginProvider
                        AND ul.[ProviderKey] = @ProviderKey
                ",
                param: new
                {
                    LoginProvider = userLoginKey.LoginProvider,
                    ProviderKey = userLoginKey.ProviderKey
                },
                transaction: Transaction
            );
        }

        public async Task<IEnumerable<User>> GetByNormalizedRoleNameAsync(string normalizedRoleName)
        {
            return await Connection.QueryAsync<User>
            (
                sql:
                @"
                    SELECT
                        u.[Id],
                        u.[UserName],
                        u.[NormalizeduserName],
                        u.[Email],
                        u.[NormalizedEmail],
                        u.[EmailConfirmed],
                        u.[PasswordHash],
                        u.[SecurityStamp],
                        u.[ConcurrencyStamp],
                        u.[PhoneNumber],
                        u.[PhoneNumberConfirmed],
                        u.[TwoFactorEnabled],
                        u.[LockoutEnd],
                        u.[LockoutEnabled],
                        u.[AccessFailedCount]
                    FROM
                        [Roles] r INNER JOIN
                        [UserRoles] ur ON r.[Id] = ur.[RoleId] INNER JOIN
                        [Users] u ON ur.[UserId] = u.[Id]
                    WHERE
                        r.[NormalizedName] = @NormalizedRoleName
                ",
                param: new
                {
                    NormalizedRoleName = normalizedRoleName
                },
                transaction: Transaction
            );
        }

        public async Task UpdateAsync(User entity)
        {
            await Connection.ExecuteAsync
            (
                sql:
                @"
                    UPDATE
                        [Users]
                    SET
                        [UserName] = @UserName,
                        [NormalizedUserName] = @NormalizedUserName,
                        [Email] = @Email,
                        [NormalizedEmail] = @NormalizedEmail,
                        [EmailConfirmed] = @EmailConfirmed,
                        [PasswordHash] = @PasswordHash,
                        [SecurityStamp] = @SecurityStamp,
                        [ConcurrencyStamp] = @ConcurrencyStamp,
                        [PhoneNumber] = @PhoneNumber,
                        [PhoneNumberConfirmed] = @PhoneNumberConfirmed,
                        [TwoFactorEnabled] = @TwoFactorEnabled,
                        [LockoutEnd] = @LockoutEnd,
                        [LockoutEnabled] = @LockoutEnabled,
                        [AccessFailedCount] = @AccessFailedCount
                    WHERE
                        [Id] = @Id
                ",
                param: new
                {
                    Id = entity.Id,
                    UserName = entity.UserName,
                    NormalizeduserName = entity.NormalizedUserName,
                    Email = entity.Email,
                    NormalizedEmail = entity.NormalizedEmail,
                    EmailConfirmed = entity.EmailConfirmed,
                    PasswordHash = entity.PasswordHash,
                    SecurityStamp = entity.SecurityStamp,
                    ConcurrencyStamp = entity.ConcurrencyStamp,
                    PhoneNumber = entity.PhoneNumber,
                    PhoneNumberConfirmed = entity.PhoneNumberConfirmed,
                    TwoFactorEnabled = entity.TwoFactorEnabled,
                    LockoutEnd = entity.LockoutEnd,
                    LockoutEnabled = entity.LockoutEnabled,
                    AccessFailedCount = entity.AccessFailedCount
                },
                transaction: Transaction
            );
        }

        public User GetByNormalizedUsername(string normalizedUsername)
        {
            return GetByNormalizedUsernameAsync(normalizedUsername).Result;
        }

        public User GetByNormalizedEmail(string normalizedEmail)
        {
            return GetByNormalizedEmailAsync(normalizedEmail).Result;
        }

        public User GetByUserLogin(UserLoginKey userLoginKey)
        {
            return GetByUserLoginAsync(userLoginKey).Result;
        }

        public IEnumerable<User> GetByNormalizedRoleName(string normalizedRoleName)
        {
            return GetByNormalizedRoleNameAsync(normalizedRoleName).Result;
        }

        public IEnumerable<User> GetAll()
        {
            return GetAllAsync().Result;
        }

        public User GetByKey(Guid key)
        {
            return GetByKeyAsync(key).Result;
        }

        public void Add(User entity)
        {
            AddAsync(entity).Wait();
        }

        public void Update(User entity)
        {
            UpdateAsync(entity).Wait();
        }

        public void Delete(User entity)
        {
            DeleteAsync(entity).Wait(0);
        }

        public void AddUserToRole(Guid userId, string normalizedRoleName)
        {
            AdduserToRoleAsync(userId, normalizedRoleName).Wait();
        }

        public async Task AdduserToRoleAsync(Guid userId, string normalizedRoleName)
        {
            var role = await Connection.QuerySingleOrDefaultAsync<Role>
            (
                sql:
                @"
                    SELECT
                        [Id],
                        [Name],
                        [NormalizedName],
                        [ConcurrencyStamp]
                    FROM
                        [Roles]
                    WHERE
                        [NormalizedName] = @NormalizedName
                ",
                param: new
                {
                    NormalizedName = normalizedRoleName
                },
                transaction: Transaction
            );

            if(role != null)
            {
                await Connection.ExecuteAsync
                (
                    sql:
                    @"
                        INSERT INTO [UserRoles] (
                            [UserId],
                            [RoleId]
                        ) VALUES (
                            @UserId,
                            @RoleId
                        )
                    ",
                    param: new
                    {
                        UserId = userId,
                        RoleId = role.Id
                    },
                    transaction: Transaction
                );
            }
        }

        public IList<User> GetByClaim(string claimType, string claimValue)
        {


            throw new NotImplementedException();
        }

        public async Task<IList<User>> GetByClaimAsync(string claimType, string claimValue)
        {
            return (await Connection.QueryAsync<User>
            (
                sql:
                @"
                    SELECT
                        u.[Id],
                        u.[UserName],
                        u.[NormalizeduserName],
                        u.[Email],
                        u.[NormalizedEmail],
                        u.[EmailConfirmed],
                        u.[PasswordHash],
                        u.[SecurityStamp],
                        u.[ConcurrencyStamp],
                        u.[PhoneNumber],
                        u.[PhoneNumberConfirmed],
                        u.[TwoFactorEnabled],
                        u.[LockoutEnd],
                        u.[LockoutEnabled],
                        u.[AccessFailedCount]
                    FROM
                        [UserClaims] uc INNER JOIN
                        [Users] u ON uc.[UserId] = u.[Id]
                    WHERE
                        uc.[Type] = @ClaimType
                        AND uc.[Value] = @ClaimValue
                ",
                param: new
                {
                    ClaimType = claimType,
                    ClaimValue = claimValue
                },
                transaction: Transaction
            )).ToList();
        }

        public void RemoveUserFromRole(Guid userId, string normalizedRoleName)
        {
            RemoveuserFromRoleAsync(userId, normalizedRoleName).Wait();
        }

        public async Task RemoveuserFromRoleAsync(Guid userId, string normalizedRoleName)
        {
            var roleIdStr = await Connection.ExecuteScalarAsync<string>(
                sql: @"
                    SELECT
                        [Id]
                    FROM
                        [Roles]
                    WHERE
                        [NormalizedName] = @NormalizedRoleName
                ",
                param: new {
                    NormalizedRoleName = normalizedRoleName
                },
                transaction: Transaction
            );

            var roleId = getGuid(roleIdStr);

            await Connection.ExecuteAsync(
                sql: @"
                    DELETE FROM
                        [UserRoles]
                    WHERE
                        [UserId] = @UserId AND
                        [RoleId] = @RoleId
                ",
                param: new {
                    UserId = userId,
                    RoleId = roleId
                },
                transaction: Transaction
            );
        }

        private Guid getGuid(string value)
        {
            var result = default(Guid);
            Guid.TryParse(value, out result);
            return result;
        }

    }
}

