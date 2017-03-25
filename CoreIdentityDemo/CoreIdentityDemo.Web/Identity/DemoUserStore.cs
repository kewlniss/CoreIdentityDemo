using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Security.Claims;
using CoreIdentityDemo.ApiClients;
using CoreIdentityDemo.Common.Util;
using CoreIdentityDemo.Common.IdentityApi;

namespace CoreIdentityDemo.Web.Identity
{
    public class DemoUserStore
        : IUserLoginStore<DemoIdentityUser>
        , IUserRoleStore<DemoIdentityUser>
        , IUserClaimStore<DemoIdentityUser>
        , IUserPasswordStore<DemoIdentityUser>
        , IUserSecurityStampStore<DemoIdentityUser>
        , IUserEmailStore<DemoIdentityUser>
        , IUserLockoutStore<DemoIdentityUser>
        , IUserPhoneNumberStore<DemoIdentityUser>
        , IUserTwoFactorStore<DemoIdentityUser>
        , IUserAuthenticationTokenStore<DemoIdentityUser>
        , IUserStore<DemoIdentityUser>
    {
        private readonly IIdentityApiClient _identityApiClient;
        private bool _disposed;

        public DemoUserStore(IIdentityApiClient identityApiClient)
        {
            _identityApiClient = identityApiClient;
        }

        public async Task AddClaimsAsync(DemoIdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            ExceptionUtil.ThrowIfNull(claims, nameof(claims));

            var userId = getGuid(user.Id);
            foreach(var claim in claims)
            {
                await _identityApiClient.AddUserClaimAsync(userId, claim.Type, claim.Value);
            }
        }

        public async Task AddLoginAsync(DemoIdentityUser user, UserLoginInfo login, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            ExceptionUtil.ThrowIfNull(login, nameof(login));

            var userId = getGuid(user.Id);
            await _identityApiClient.AddUserLoginAsync(userId, login.LoginProvider, login.ProviderKey, login.ProviderDisplayName);
        }

        public async Task AddToRoleAsync(DemoIdentityUser user, string normalizedRoleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            ExceptionUtil.ThrowIfNullOrWhiteSpace(normalizedRoleName, nameof(normalizedRoleName));

            var userId = getGuid(user.Id);
            await _identityApiClient.AddUserToRoleAsync(userId, normalizedRoleName);
        }

        public async Task<IdentityResult> CreateAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            var model = getUserModel(user);
            await _identityApiClient.CreateUserAsync(model);
           
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            var userId = getGuid(user.Id);
            await _identityApiClient.DeleteUserAsync(userId);

            return IdentityResult.Success;
        }

        public void Dispose()
        {
            if(_identityApiClient != null)
                _identityApiClient.Dispose();
            _disposed = true;
        }

        public async Task<DemoIdentityUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();

            var model = await _identityApiClient.FindUserByEmailAsync(normalizedEmail);
            return getIdentityUser(model);
        }

        public async Task<DemoIdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();

            var userGuid = getGuid(userId);
            var model = await _identityApiClient.FindUserByIdAsync(userGuid);
            return getIdentityUser(model);
        }

        public async Task<DemoIdentityUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();

            var model = await _identityApiClient.FindUserByLoginAsync(loginProvider, providerKey);
            return getIdentityUser(model);
        }

        public async Task<DemoIdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();

            var model = await _identityApiClient.FindUserByNameAsync(normalizedUserName);
            return getIdentityUser(model);
        }

        public Task<int> GetAccessFailedCountAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            return Task.FromResult(user.AccessFailedCount);
        }

        public async Task<IList<Claim>> GetClaimsAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            var userId = getGuid(user.Id);
            return (await _identityApiClient.GetUserClaimsAsync(userId))
                .Select(x => new Claim(x.Type, x.Value)).ToList();
        }

        public Task<string> GetEmailAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            return Task.FromResult(user.EmailConfirmed);
        }

        public Task<bool> GetLockoutEnabledAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            return Task.FromResult(user.LockoutEnabled);
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            return Task.FromResult(user.LockoutEndDate);
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            var userId = getGuid(user.Id);
            return (await _identityApiClient.GetUserLoginsAsync(userId))
                .Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey, x.ProviderDisplayName)).ToList();
        }

        public Task<string> GetNormalizedEmailAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            return Task.FromResult(user.NormalizedEmail);
        }

        public Task<string> GetNormalizedUserNameAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetPasswordHashAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            return Task.FromResult(user.PasswordHash);
        }

        public Task<string> GetPhoneNumberAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public async Task<IList<string>> GetRolesAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            var userId = getGuid(user.Id);
            return await _identityApiClient.GetUserRolesAsync(userId);
        }

        public Task<string> GetSecurityStampAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            return Task.FromResult(user.SecurityStamp);
        }

        public async Task<string> GetTokenAsync(DemoIdentityUser user, string loginProvider, string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            var userId = getGuid(user.Id);
            return (await _identityApiClient.GetUserTokensAsync(userId))
                .Where(x => x.LoginProvider == loginProvider && x.Name == name)
                .Select(x => x.Value)
                .FirstOrDefault();
        }

        public Task<bool> GetTwoFactorEnabledAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task<string> GetUserIdAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            return Task.FromResult(user.UserName);
        }

        public Task<IList<DemoIdentityUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<IList<DemoIdentityUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHash));
        }

        public Task<int> IncrementAccessFailedCountAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public async Task<bool> IsInRoleAsync(DemoIdentityUser user, string normalizedRoleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            ExceptionUtil.ThrowIfNullOrWhiteSpace(normalizedRoleName, nameof(normalizedRoleName));
            var userId = getGuid(user.Id);

            return await _identityApiClient.IsUserInRole(userId, normalizedRoleName);
        }

        public async Task RemoveClaimsAsync(DemoIdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            ExceptionUtil.ThrowIfNull(claims, nameof(claims));

            var userId = getGuid(user.Id);
            foreach(var claim in claims)
            {
                await _identityApiClient.RemoveUserClaimAsync(userId, claim.Type, claim.Value);
            }
        }

        public async Task RemoveFromRoleAsync(DemoIdentityUser user, string normalizedRoleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            ExceptionUtil.ThrowIfNullOrWhiteSpace(normalizedRoleName, nameof(normalizedRoleName));
            var userId = getGuid(user.Id);

            await _identityApiClient.RemoveUserFromRoleAsync(userId, normalizedRoleName);
        }

        public async Task RemoveLoginAsync(DemoIdentityUser user, string loginProvider, string providerKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            var userId = getGuid(user.Id);

            await _identityApiClient.RemoveUserLoginAsync(userId, loginProvider, providerKey);
        }

        public async Task RemoveTokenAsync(DemoIdentityUser user, string loginProvider, string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            var userId = getGuid(user.Id);

            await _identityApiClient.RemoveUserTokenAsync(userId, loginProvider, name);
        }

        public async Task ReplaceClaimAsync(DemoIdentityUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            ExceptionUtil.ThrowIfNull(claim, nameof(claim));
            ExceptionUtil.ThrowIfNull(newClaim, nameof(newClaim));
            var userId = getGuid(user.Id);
            var claimModel = new ClaimModel { Type = claim.Type, Value = claim.Value };
            var newClaimModel = new ClaimModel { Type = newClaim.Type, Value = newClaim.Value };

            await _identityApiClient.ReplaceUserClaimAsync(userId, claimModel, newClaimModel);
        }

        public Task ResetAccessFailedCountAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            user.AccessFailedCount = 0;
            return Task.CompletedTask;
        }

        public Task SetEmailAsync(DemoIdentityUser user, string email, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            user.Email = email;
            return Task.CompletedTask;
        }

        public Task SetEmailConfirmedAsync(DemoIdentityUser user, bool confirmed, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public Task SetLockoutEnabledAsync(DemoIdentityUser user, bool enabled, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            user.LockoutEnabled = enabled;
            return Task.CompletedTask;
        }

        public Task SetLockoutEndDateAsync(DemoIdentityUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            user.LockoutEndDate = lockoutEnd;
            return Task.CompletedTask;
        }

        public Task SetNormalizedEmailAsync(DemoIdentityUser user, string normalizedEmail, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            user.NormalizedEmail = normalizedEmail;
            return Task.CompletedTask;
        }

        public Task SetNormalizedUserNameAsync(DemoIdentityUser user, string normalizedName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(DemoIdentityUser user, string passwordHash, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task SetPhoneNumberAsync(DemoIdentityUser user, string phoneNumber, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            user.PhoneNumber = phoneNumber;
            return Task.CompletedTask;
        }

        public Task SetPhoneNumberConfirmedAsync(DemoIdentityUser user, bool confirmed, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            user.PhoneNumberConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public Task SetSecurityStampAsync(DemoIdentityUser user, string stamp, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            user.SecurityStamp = stamp;
            return Task.CompletedTask;

        }

        public async Task SetTokenAsync(DemoIdentityUser user, string loginProvider, string name, string value, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            var userId = getGuid(user.Id);

            await _identityApiClient.SetUserTokenAsync(userId, loginProvider, name, value);
        }

        public Task SetTwoFactorEnabledAsync(DemoIdentityUser user, bool enabled, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            user.TwoFactorEnabled = enabled;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(DemoIdentityUser user, string userName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            var model = getUserModel(user);
            await _identityApiClient.UpdateUserAsync(model);

            return IdentityResult.Success;
        }

        private void throwIfDisposed()
        {
            if(_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        private Guid getGuid(string value)
        {
            Guid.TryParse(value, out Guid result);
            return result;
        }

        private UserModel getUserModel(DemoIdentityUser user)
        {
            if (user == null)
                return null;

            var model = new UserModel();
            populateUserModel(model, user);
            return model;
        }

        private void populateUserModel(UserModel model, DemoIdentityUser user)
        {
            ExceptionUtil.ThrowIfNull(model, nameof(model));
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            model.Id = getGuid(user.Id);
            model.UserName = user.UserName;
            model.NormalizedUserName = user.NormalizedUserName;
            model.Email = user.Email;
            model.NormalizedEmail = user.NormalizedEmail;
            model.EmailConfirmed = user.EmailConfirmed;
            model.PasswordHash = user.PasswordHash;
            model.SecurityStamp = user.SecurityStamp;
            model.ConcurrencyStamp = user.ConcurrencyStamp;
            model.PhoneNumber = user.PhoneNumber;
            model.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            model.TwoFactorEnabled = user.TwoFactorEnabled;
            model.LockoutEnd = user.LockoutEndDate;
            model.LockoutEnabled = user.LockoutEnabled;
            model.AccessFailedCount = user.AccessFailedCount;
        }

        private DemoIdentityUser getIdentityUser(UserModel model)
        {
            if (model == null)
                return null;

            var user = new DemoIdentityUser();
            populateIdentityUser(user, model);
            return user;
        }

        private void populateIdentityUser(DemoIdentityUser user, UserModel model)
        {
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            ExceptionUtil.ThrowIfNull(model, nameof(model));

            user.Id = model.Id.ToString();
            user.UserName = model.UserName;
            user.NormalizedUserName = model.NormalizedUserName;
            user.Email = model.Email;
            user.NormalizedEmail = model.NormalizedEmail;
            user.EmailConfirmed = model.EmailConfirmed;
            user.PasswordHash = model.PasswordHash;
            user.SecurityStamp = model.SecurityStamp;
            user.ConcurrencyStamp = model.ConcurrencyStamp;
            user.PhoneNumber = model.PhoneNumber;
            user.PhoneNumberConfirmed = model.PhoneNumberConfirmed;
            user.TwoFactorEnabled = model.TwoFactorEnabled;
            user.LockoutEndDate = model.LockoutEnd;
            user.LockoutEnabled = model.LockoutEnabled;
            user.AccessFailedCount = model.AccessFailedCount;
        }
    }
}
