using CoreIdentityDemo.Domain.Entities;
using CoreIdentityDemo.Domain.Keys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreIdentityDemo.Domain.Repositories
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        User GetByNormalizedUsername(string normalizedUsername);
        Task<User> GetByNormalizedUsernameAsync(string normalizedUsername);

        User GetByNormalizedEmail(string normalizedEmail);
        Task<User> GetByNormalizedEmailAsync(string normalizedEmail);

        User GetByUserLogin(UserLoginKey userLoginKey);
        Task<User> GetByUserLoginAsync(UserLoginKey userLoginKey);

        IEnumerable<User> GetByNormalizedRoleName(string normalizedRoleName);
        Task<IEnumerable<User>> GetByNormalizedRoleNameAsync(string normalizedRoleName);

        void AddUserToRole(Guid userId, string normalizedRoleName);
        Task AdduserToRoleAsync(Guid userId, string normalizedRoleName);

        IList<User> GetByClaim(string claimType, string claimValue);
        Task<IList<User>> GetByClaimAsync(string claimType, string claimValue);

        void RemoveUserFromRole(Guid userId, string normalizedRoleName);
        Task RemoveuserFromRoleAsync(Guid userId, string normalizedRoleName);
    }
}
