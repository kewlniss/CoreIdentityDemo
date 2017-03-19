using CoreIdentityDemo.Common.IdentityApi;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CoreIdentityDemo.ApiClients
{
    public class IdentityApiClient : BaseApiClient, IIdentityApiClient
    {
        public IdentityApiClient(string baseAddress)
            : base(baseAddress)
        { }

        public async Task AddUserClaimAsync(Guid userId, string claimType, string claimValue)
        {
            var args = new ClaimModel
            {
                Type = claimType,
                Value = claimValue
            };
            await PostAsync($"/api/identity/user/{userId}/claim", args);
        }

        public async Task AddUserLoginAsync(Guid userId, string loginProvider, string providerKey, string providerDisplayName)
        {
            var args = new LoginModel
            {
                LoginProvider = loginProvider,
                ProviderKey = providerKey,
                ProviderDisplayName = providerDisplayName
            };
            await PostAsync($"/api/identity/user/{userId}/login", args);
        }

        public async Task AddUserToRoleAsync(Guid userId, string roleName)
        {
            await PostAsync("/api/identity/user/{userId}/role", new { RoleName = roleName });
        }

        public async Task CreateUserAsync(UserModel args)
        {
            await PostAsync("/api/identity/user", args);
        }

        public async Task DeleteUserAsync(Guid userId)
        {
            await DeleteAsync($"/api/identity/user/{userId}");
        }

        public async Task<UserModel> FindUserByEmailAsync(string normalizedEmail)
        {
            var encNormalizedEmail = WebUtility.UrlEncode(normalizedEmail);
            return await GetAsync<UserModel>($"/api/identity/user/email?normalizedEmail={encNormalizedEmail}");
        }

        public async Task<UserModel> FindUserByIdAsync(Guid userId)
        {
            return await GetAsync<UserModel>($"/api/identity/user/{userId}");
        }

        public async Task<UserModel> FindUserByLoginAsync(string loginProvider, string providerKey)
        {
            var encLoginProvider = WebUtility.UrlEncode(loginProvider);
            var encProviderKey = WebUtility.UrlEncode(providerKey);
            return await GetAsync<UserModel>($"/api/identity/user/login?loginProvider={encLoginProvider}&providerKey={encProviderKey}");
        }

        public async Task<UserModel> FindUserByNameAsync(string normalizedUserName)
        {
            var encNormalizedUserName = WebUtility.UrlEncode(normalizedUserName);
            return await GetAsync<UserModel>($"/api/identity/user/name?normalizedUserName={encNormalizedUserName}");
        }

        public async Task<ClaimModel[]> GetUserClaimsAsync(Guid userId)
        {
            return await GetAsync<ClaimModel[]>($"/api/identity/user/{userId}/claims");
        }

        public async Task<LoginModel[]> GetUserLoginsAsync(Guid userId)
        {
            return await GetAsync<LoginModel[]>($"/api/identity/user/{userId}/logins");
        }

        public async Task<string[]> GetUserRolesAsync(Guid userId)
        {
            return await GetAsync<string[]>($"/api/identity/user/{userId}/roles");
        }

        public async Task<TokenModel[]> GetUserTokensAsync(Guid userId)
        {
            return await GetAsync<TokenModel[]>($"/api/identity/user/{userId}/tokens");
        }

        public async Task<UserModel[]> GetUsersForClaimAsync(string claimType, string claimValue)
        {
            var encClaimType = WebUtility.UrlEncode(claimType);
            var encClaimValue = WebUtility.UrlEncode(claimValue);
            return await GetAsync<UserModel[]>($"/api/identity/user/claim?claimType={encClaimType}&claimValue={encClaimValue}");
        }

        public async Task<UserModel[]> GetUsersInRole(string roleName)
        {
            var encRoleName = WebUtility.UrlEncode(roleName);
            return await GetAsync<UserModel[]>($"/api/identity/user/role?roleName={encRoleName}");
        }

        public async Task<bool> IsUserInRole(Guid userId, string roleName)
        {
            var roles = await GetUserRolesAsync(userId);
            return roles.Any(x => string.Compare(x, roleName, true) == 0);
        }

        public async Task RemoveUserClaimAsync(Guid userId, string claimType, string claimValue)
        {
            var encClaimType = WebUtility.UrlEncode(claimType);
            var encClaimValue = WebUtility.UrlEncode(claimValue);
            await DeleteAsync($"/api/identity/user/{userId}/claim?claimType={encClaimType}&claimValue={encClaimValue}");
        }

        public async Task RemoveUserFromRoleAsync(Guid userId, string roleName)
        {
            var encRoleName = WebUtility.UrlEncode(roleName);
            await DeleteAsync($"/api/identity/user/{userId}/role?roleName={encRoleName}");
        }

        public async Task RemoveUserLoginAsync(Guid userId, string loginProvider, string providerKey)
        {
            var encLoginProvider = WebUtility.UrlEncode(loginProvider);
            var encProviderKey = WebUtility.UrlEncode(providerKey);
            await DeleteAsync($"/api/identity/user/{userId}/login?loginProvider={encLoginProvider}&providerKey={encProviderKey}");
        }

        public async Task RemoveUserTokenAsync(Guid userId, string loginProvider, string name)
        {
            var encLoginProvider = WebUtility.UrlEncode(loginProvider);
            var encName = WebUtility.UrlEncode(name);
            await DeleteAsync($"/api/identity/user/{userId}/token?loginProvider={encLoginProvider}&name={encName}");
        }

        public async Task ReplaceUserClaimAsync(Guid userId, ClaimModel claim, ClaimModel newClaim)
        {
            await PutAsync($"/api/identity/user/{userId}/claim", new { Claim = claim, NewClaim = newClaim });
        }

        public async Task SetUserTokenAsync(Guid userId, string loginProvider, string name, string value)
        {
            var args = new TokenModel
            {
                LoginProvider = loginProvider,
                Name = name,
                Value = value
            };
            await PostAsync($"/api/identity/user/{userId}/token", args);
        }

        public async Task UpdateUserAsync(UserModel args)
        {
            await PutAsync("/api/identity/user", args);
        }

        public async Task AddRoleClaimAsync(Guid roleId, string claimType, string claimValue)
        {
            var args = new ClaimModel
            {
                Type = claimType,
                Value = claimValue
            };
            await PostAsync($"/api/identity/role/{roleId}/claim", args);
        }

        public async Task CreateRoleAsync(RoleModel args)
        {
            await PostAsync($"/api/identity/role", args);
        }

        public async Task DeleteRoleAsync(Guid roleId)
        {
            await DeleteAsync($"/api/identity/role/{roleId}");
        }

        public async Task<RoleModel> FindRoleByIdAsync(Guid roleId)
        {
            return await GetAsync<RoleModel>($"/api/identity/role/{roleId}");
        }

        public async Task<RoleModel> FindRoleByName(string normalizedRoleName)
        {
            var encNormalizedRoleName = WebUtility.UrlEncode(normalizedRoleName);
            return await GetAsync<RoleModel>($"/api/identity/role/name?normalizedName={encNormalizedRoleName}");
        }

        public async Task<ClaimModel[]> GetRoleClaimsAsync(Guid roleId)
        {
            return await GetAsync<ClaimModel[]>($"/api/identity/role/{roleId}/claims");
        }

        public async Task RemoveRoleClaimAsync(Guid roleId, string claimType, string claimValue)
        {
            var encClaimType = WebUtility.UrlEncode(claimType);
            var encClaimValue = WebUtility.UrlEncode(claimValue);
            await DeleteAsync($"/api/identity/role/{roleId}/claim?claimType={encClaimType}&claimValue={encClaimValue}");
        }

        public async Task UpdateRole(RoleModel args)
        {
            await PutAsync("/api/identity/role", args);
        }
    }
}
