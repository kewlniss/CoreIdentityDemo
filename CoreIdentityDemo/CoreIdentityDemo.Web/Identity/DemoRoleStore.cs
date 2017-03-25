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
    public class DemoRoleStore
        : IRoleStore<DemoIdentityRole>
        , IRoleClaimStore<DemoIdentityRole>
    {
        private readonly IIdentityApiClient _identityApiClient;
        private bool _disposed;

        public DemoRoleStore(IIdentityApiClient identityApiClient)
        {
            _identityApiClient = identityApiClient;
        }

        public async Task AddClaimAsync(DemoIdentityRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(role, nameof(role));
            ExceptionUtil.ThrowIfNull(claim, nameof(claim));

            var roleId = getGuid(role.Id);
            await _identityApiClient.AddRoleClaimAsync(roleId, claim.Type, claim.Value);
        }

        public async Task<IdentityResult> CreateAsync(DemoIdentityRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(role, nameof(role));

            var model = getRoleModel(role);
            await _identityApiClient.CreateRoleAsync(model);

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(DemoIdentityRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(role, nameof(role));

            var roleId = getGuid(role.Id);
            await _identityApiClient.DeleteRoleAsync(roleId);

            return IdentityResult.Success;
        }

        public void Dispose()
        {
            if (_identityApiClient != null)
                _identityApiClient.Dispose();
            _disposed = true;
        }

        public async Task<DemoIdentityRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();

            var roleGuid = getGuid(roleId);
            var model = await _identityApiClient.FindRoleByIdAsync(roleGuid);
            return getIdentityRole(model);
        }

        public async Task<DemoIdentityRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();

            var model = await _identityApiClient.FindRoleByName(normalizedRoleName);
            return getIdentityRole(model);
        }

        public async Task<IList<Claim>> GetClaimsAsync(DemoIdentityRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(role, nameof(role));

            var roleId = getGuid(role.Id);
            return (await _identityApiClient.GetRoleClaimsAsync(roleId))
                .Select(x => new Claim(x.Type, x.Value))
                .ToList();
        }

        public Task<string> GetNormalizedRoleNameAsync(DemoIdentityRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(role, nameof(role));

            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(DemoIdentityRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(role, nameof(role));

            return Task.FromResult(role.Id);
        }

        public Task<string> GetRoleNameAsync(DemoIdentityRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(role, nameof(role));

            return Task.FromResult(role.Name);
        }

        public async Task RemoveClaimAsync(DemoIdentityRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(role, nameof(role));
            ExceptionUtil.ThrowIfNull(claim, nameof(claim));

            var roleId = getGuid(role.Id);
            await _identityApiClient.RemoveRoleClaimAsync(roleId, claim.Type, claim.Value);
        }

        public Task SetNormalizedRoleNameAsync(DemoIdentityRole role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(role, nameof(role));

            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(DemoIdentityRole role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(role, nameof(role));

            role.Name = roleName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(DemoIdentityRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(role, nameof(role));

            role.ConcurrencyStamp = Guid.NewGuid().ToString();
            var model = getRoleModel(role);
            await _identityApiClient.UpdateRole(model);

            return IdentityResult.Success;
        }

        private void throwIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        private Guid getGuid(string value)
        {
            Guid.TryParse(value, out Guid result);
            return result;
        }

        private RoleModel getRoleModel(DemoIdentityRole role)
        {
            var model = new RoleModel();
            populateRoleModel(model, role);
            return model;
        }

        private void populateRoleModel(RoleModel model, DemoIdentityRole role)
        {
            model.Id = getGuid(role.Id);
            model.Name = role.Name;
            model.NormalizedName = role.NormalizedName;
            model.ConcurrencyStamp = role.ConcurrencyStamp;
        }

        private DemoIdentityRole getIdentityRole(RoleModel model)
        {
            var role = new DemoIdentityRole();
            populateIdentityRole(role, model);
            return role;
        }

        private void populateIdentityRole(DemoIdentityRole role, RoleModel model)
        {
            role.Id = model.Id.ToString();
            role.Name = model.Name;
            role.NormalizedName = model.NormalizedName;
            role.ConcurrencyStamp = model.ConcurrencyStamp;
        }
    }
}
