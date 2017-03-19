using System;
using System.Threading.Tasks;
using CoreIdentityDemo.Common.IdentityApi;

namespace CoreIdentityDemo.ApiClients
{
    public interface IIdentityApiClient : IDisposable
    {
        Task AddRoleClaimAsync(Guid roleId, string claimType, string claimValue);
        Task AddUserToRoleAsync(Guid userId, string roleName);
        Task AddUserClaimAsync(Guid userId, string claimType, string claimValue);
        Task AddUserLoginAsync(Guid userId, string loginProvider, string providerKey, string providerDisplayName);
        Task CreateUserAsync(UserModel args);
        Task CreateRoleAsync(RoleModel args);
        Task DeleteUserAsync(Guid userId);
        Task DeleteRoleAsync(Guid roleId);
        Task<RoleModel> FindRoleByName(string normalizedRoleName);
        Task<RoleModel> FindRoleByIdAsync(Guid roleId);
        Task<UserModel> FindUserByEmailAsync(string normalizedEmail);
        Task<UserModel> FindUserByIdAsync(Guid userId);
        Task<UserModel> FindUserByLoginAsync(string loginProvider, string providerKey);
        Task<UserModel> FindUserByNameAsync(string normalizedUserName);
        Task<ClaimModel[]> GetRoleClaimsAsync(Guid roleId);
        Task<ClaimModel[]> GetUserClaimsAsync(Guid userId);
        Task<LoginModel[]> GetUserLoginsAsync(Guid userId);
        Task<string[]> GetUserRolesAsync(Guid userId);
        Task<UserModel[]> GetUsersForClaimAsync(string claimType, string claimValue);
        Task<UserModel[]> GetUsersInRole(string roleName);
        Task<TokenModel[]> GetUserTokensAsync(Guid userId);
        Task<bool> IsUserInRole(Guid userId, string roleName);
        Task RemoveRoleClaimAsync(Guid roleId, string claimType, string claimValue);
        Task RemoveUserClaimAsync(Guid userId, string claimType, string claimValue);
        Task RemoveUserFromRoleAsync(Guid userId, string roleName);
        Task RemoveUserLoginAsync(Guid userId, string loginProvider, string providerKey);
        Task RemoveUserTokenAsync(Guid userId, string loginProvider, string name);
        Task ReplaceUserClaimAsync(Guid userId, ClaimModel claim, ClaimModel newClaim);
        Task SetUserTokenAsync(Guid userId, string loginProvider, string name, string value);
        Task UpdateRole(RoleModel args);
        Task UpdateUserAsync(UserModel args);
    }
}