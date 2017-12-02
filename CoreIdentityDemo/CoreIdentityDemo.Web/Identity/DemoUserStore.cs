using CoreIdentityDemo.Domain.Entities;
using CoreIdentityDemo.Domain.Keys;
using CoreIdentityDemo.Domain.UnitOfWork;
using CoreIdentityDemo.Web.Util;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        private readonly IUnitOfWork _unitOfWork;
        private bool _disposed;

        public DemoUserStore(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddClaimsAsync(DemoIdentityUser user, IEnumerable<System.Security.Claims.Claim> claims, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            ExceptionUtil.ThrowIfNull(claims, nameof(claims));

            var userId = getGuid(user.Id);
            foreach(var claim in claims)
            {
                var userClaim = new UserClaim
                {
                    UserId = userId,
                    Type = claim.Type,
                    Value = claim.Value
                };
                await _unitOfWork.UserClaimRepository.AddAsync(userClaim);
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task AddLoginAsync(DemoIdentityUser user, UserLoginInfo login, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            ExceptionUtil.ThrowIfNull(login, nameof(login));

            var userLogin = new UserLogin
            {
                LoginProvider = login.LoginProvider,
                ProviderDisplayName = login.ProviderDisplayName,
                ProviderKey = login.ProviderKey,
                UserId = getGuid(user.Id)
            };

            await _unitOfWork.UserLoginRepository.AddAsync(userLogin);
            await _unitOfWork.CommitAsync();
        }

        public async Task AddToRoleAsync(DemoIdentityUser user, string normalizedRoleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            ExceptionUtil.ThrowIfNullOrWhiteSpace(normalizedRoleName, nameof(normalizedRoleName));

            var userId = getGuid(user.Id);

            await _unitOfWork.UserRepository.AdduserToRoleAsync(userId, normalizedRoleName);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IdentityResult> CreateAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            var u = getUser(user);

            await _unitOfWork.UserRepository.AddAsync(u);
            await _unitOfWork.CommitAsync();
            
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            var u = getUser(user);

            await _unitOfWork.UserRepository.DeleteAsync(u);
            await _unitOfWork.CommitAsync();

            return IdentityResult.Success;
        }

        public void Dispose()
        {
            if(_unitOfWork != null)
                _unitOfWork.Dispose();
            _disposed = true;
        }

        public async Task<DemoIdentityUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();

            var user = await _unitOfWork.UserRepository.GetByNormalizedEmailAsync(normalizedEmail);
            var result = getIdentityUser(user);

            return result;
        }

        public async Task<DemoIdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();

            var userGuid = getGuid(userId);
            var user = await _unitOfWork.UserRepository.GetByKeyAsync(userGuid);
            var result = getIdentityUser(user);

            return result;
        }

        public async Task<DemoIdentityUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();

            var loginKey = new UserLoginKey
            {
                LoginProvider = loginProvider,
                ProviderKey = providerKey
            };

            var user = await _unitOfWork.UserRepository.GetByUserLoginAsync(loginKey);
            var result = getIdentityUser(user);

            return result;
        }

        public async Task<DemoIdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();

            var user = await _unitOfWork.UserRepository.GetByNormalizedUsernameAsync(normalizedUserName);
            var result = getIdentityUser(user);

            return result;
        }

        public Task<int> GetAccessFailedCountAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            return Task.FromResult(user.AccessFailedCount);
        }

        public async Task<IList<System.Security.Claims.Claim>> GetClaimsAsync(DemoIdentityUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));

            var userId = getGuid(user.Id);
            var result = (await _unitOfWork.UserClaimRepository.GetByUserIdAsync(userId))
                .Select(x => new System.Security.Claims.Claim( x.Type, x.Value))
                .ToList();

            return result;
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
            var result = (await _unitOfWork.UserLoginRepository.GetByUserIdAsync(userId))
                .Select(x => new UserLoginInfo(x.LoginProvider, x.ProviderKey, x.ProviderDisplayName))
                .ToList();

            return result;
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
            var result = (await _unitOfWork.RoleRepository.GetByUserIdAsync(userId))
                .Select(x => x.Name)
                .ToList();

            return result;
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
            var token = await _unitOfWork.UserTokenRepository.GetByKeyAsync(null);
            var result = token?.Value;

            return result;
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

        public async Task<IList<DemoIdentityUser>> GetUsersForClaimAsync(System.Security.Claims.Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();

            var result = (await _unitOfWork.UserRepository.GetByClaimAsync(claim.Type, claim.Value))
                .Select(x => getIdentityUser(x))
                .ToList();

            return result;
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

            return (await _unitOfWork.RoleRepository.GetByUserIdAsync(userId))
                .Select(x => x.NormalizedName)
                .Any(x => x == normalizedRoleName);
        }

        public async Task RemoveClaimsAsync(DemoIdentityUser user, IEnumerable<System.Security.Claims.Claim> claims, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            ExceptionUtil.ThrowIfNull(claims, nameof(claims));

            var userId = getGuid(user.Id);
            foreach(var claim in claims)
            {
                var c = (await _unitOfWork.UserClaimRepository.GetByUserIdAsync(userId))
                    .FirstOrDefault(x => x.Type == claim.Type && x.Value == claim.Value);

                if (c != null)
                {
                    await _unitOfWork.UserClaimRepository.DeleteAsync(c);
                }
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveFromRoleAsync(DemoIdentityUser user, string normalizedRoleName, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            ExceptionUtil.ThrowIfNullOrWhiteSpace(normalizedRoleName, nameof(normalizedRoleName));
            var userId = getGuid(user.Id);

            await _unitOfWork.UserRepository.RemoveuserFromRoleAsync(userId, normalizedRoleName);
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveLoginAsync(DemoIdentityUser user, string loginProvider, string providerKey, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            var userId = getGuid(user.Id);

            var userLoginKey = new UserLoginKey { LoginProvider = loginProvider, ProviderKey = providerKey };
            var userLogin = await _unitOfWork.UserLoginRepository.GetByKeyAsync(userLoginKey);

            if (userLogin != null && userLogin.UserId == userId)
            {
                await _unitOfWork.UserLoginRepository.DeleteAsync(userLogin);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task RemoveTokenAsync(DemoIdentityUser user, string loginProvider, string name, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            var userId = getGuid(user.Id);

            var tokenKey = new UserTokenKey { LoginProvider = loginProvider, Name = name, UserId = userId };
            var token = await _unitOfWork.UserTokenRepository.GetByKeyAsync(tokenKey);

            if(token != null)
            {
                await _unitOfWork.UserTokenRepository.DeleteAsync(token);
                await _unitOfWork.CommitAsync();
            }
        }

        public async Task ReplaceClaimAsync(DemoIdentityUser user, System.Security.Claims.Claim claim, System.Security.Claims.Claim newClaim, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            ExceptionUtil.ThrowIfNull(claim, nameof(claim));
            ExceptionUtil.ThrowIfNull(newClaim, nameof(newClaim));

            var userId = getGuid(user.Id);

            var userClaim = (await _unitOfWork.UserClaimRepository.GetByUserIdAsync(userId))
                .SingleOrDefault(x => x.Type == claim.Type && x.Value == claim.Value);

            if(userClaim != null)
            {
                userClaim.Type = newClaim.Type;
                userClaim.Value = newClaim.Value;

                await _unitOfWork.UserClaimRepository.UpdateAsync(userClaim);
                await _unitOfWork.CommitAsync();
            }
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
            var userTokenKey = new UserTokenKey { LoginProvider = loginProvider, Name = name, UserId = userId };
            var userToken = await _unitOfWork.UserTokenRepository.GetByKeyAsync(userTokenKey);

            if(userToken == null)
            {
                userToken = new UserToken
                {
                    LoginProvider = loginProvider,
                    Name = name,
                    UserId = userId,
                    Value = value
                };
                await _unitOfWork.UserTokenRepository.AddAsync(userToken);
            }
            else
            {
                userToken.Value = value;
                await _unitOfWork.UserTokenRepository.UpdateAsync(userToken);
            }

            await _unitOfWork.CommitAsync();
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

            var userEntity = getUser(user);

            await _unitOfWork.UserRepository.UpdateAsync(userEntity);
            await _unitOfWork.CommitAsync();

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

        private User getUser(DemoIdentityUser user)
        {
            var result = new User();
            populateUser(result, user);
            return result;
        }

        private void populateUser(User entity, DemoIdentityUser user)
        {
            entity.AccessFailedCount = user.AccessFailedCount;
            entity.ConcurrencyStamp = user.ConcurrencyStamp;
            entity.Email = user.Email;
            entity.EmailConfirmed = user.EmailConfirmed;
            entity.Id = getGuid(user.Id);
            entity.LockoutEnabled = user.LockoutEnabled;
            entity.LockoutEnd = user.LockoutEndDate;
            entity.NormalizedEmail = user.NormalizedEmail;
            entity.NormalizedUserName = user.NormalizedUserName;
            entity.PasswordHash = user.PasswordHash;
            entity.PhoneNumber = user.PhoneNumber;
            entity.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
            entity.SecurityStamp = user.SecurityStamp;
            entity.TwoFactorEnabled = user.TwoFactorEnabled;
            entity.UserName = user.UserName;
        }

        private DemoIdentityUser getIdentityUser(User entity)
        {
            if (entity == null)
                return null;

            var user = new DemoIdentityUser();
            populateIdentityUser(user, entity);
            return user;
        }

        private void populateIdentityUser(DemoIdentityUser user, User entity)
        {
            ExceptionUtil.ThrowIfNull(user, nameof(user));
            ExceptionUtil.ThrowIfNull(entity, nameof(entity));

            user.Id = entity.Id.ToString();
            user.UserName = entity.UserName;
            user.NormalizedUserName = entity.NormalizedUserName;
            user.Email = entity.Email;
            user.NormalizedEmail = entity.NormalizedEmail;
            user.EmailConfirmed = entity.EmailConfirmed;
            user.PasswordHash = entity.PasswordHash;
            user.SecurityStamp = entity.SecurityStamp;
            user.ConcurrencyStamp = entity.ConcurrencyStamp;
            user.PhoneNumber = entity.PhoneNumber;
            user.PhoneNumberConfirmed = entity.PhoneNumberConfirmed;
            user.TwoFactorEnabled = entity.TwoFactorEnabled;
            user.LockoutEndDate = entity.LockoutEnd;
            user.LockoutEnabled = entity.LockoutEnabled;
            user.AccessFailedCount = entity.AccessFailedCount;
        }
    }
}
