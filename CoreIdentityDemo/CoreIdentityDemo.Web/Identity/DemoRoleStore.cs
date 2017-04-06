using CoreIdentityDemo.Domain.Entities;
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
    public class DemoRoleStore
        : IRoleStore<DemoIdentityRole>
        , IRoleClaimStore<DemoIdentityRole>
    {
        private readonly IUnitOfWork _unitOfWork;
        private bool _disposed;

        public DemoRoleStore(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddClaimAsync(DemoIdentityRole role, System.Security.Claims.Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(role, nameof(role));
            ExceptionUtil.ThrowIfNull(claim, nameof(claim));

            var roleId = getGuid(role.Id);

            var roleClaim = new RoleClaim
            {
                RoleId = roleId,
                Type = claim.Type,
                Value = claim.Value
            };

            await _unitOfWork.RoleClaimRepository.AddAsync(roleClaim);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IdentityResult> CreateAsync(DemoIdentityRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(role, nameof(role));

            var roleEntity = getRoleEntity(role);

            await _unitOfWork.RoleRepository.AddAsync(roleEntity);
            await _unitOfWork.CommitAsync();

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(DemoIdentityRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(role, nameof(role));

            var roleId = getGuid(role.Id);

            var roleEntity = await _unitOfWork.RoleRepository.GetByKeyAsync(roleId);
            if(roleEntity != null)
            {
                await _unitOfWork.RoleRepository.DeleteAsync(roleEntity);
                await _unitOfWork.CommitAsync();
            }

            return IdentityResult.Success;
        }

        public void Dispose()
        {
            if (_unitOfWork != null)
                _unitOfWork.Dispose();
            _disposed = true;
        }

        public async Task<DemoIdentityRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();

            var roleGuid = getGuid(roleId);
            var roleEntity = await _unitOfWork.RoleRepository.GetByKeyAsync(roleGuid);

            return getIdentityRole(roleEntity);
        }

        public async Task<DemoIdentityRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();

            var roleEntity = await _unitOfWork.RoleRepository.GetByNormalizedNameAsync(normalizedRoleName);

            return getIdentityRole(roleEntity);
        }

        public async Task<IList<System.Security.Claims.Claim>> GetClaimsAsync(DemoIdentityRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(role, nameof(role));

            var roleId = getGuid(role.Id);

            var result = (await _unitOfWork.RoleClaimRepository.GetByRoleIdAsync(roleId))
                .Select(x => new System.Security.Claims.Claim(x.Type, x.Value))
                .ToList();

            return result;
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

        public async Task RemoveClaimAsync(DemoIdentityRole role, System.Security.Claims.Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            cancellationToken.ThrowIfCancellationRequested();
            throwIfDisposed();
            ExceptionUtil.ThrowIfNull(role, nameof(role));
            ExceptionUtil.ThrowIfNull(claim, nameof(claim));

            var roleId = getGuid(role.Id);

            var roleClaim = (await _unitOfWork.RoleClaimRepository.GetByRoleIdAsync(roleId))
                .SingleOrDefault(x => x.Type == claim.Type && x.Value == claim.Value);

            if(roleClaim != null)
            {
                await _unitOfWork.RoleClaimRepository.DeleteAsync(roleClaim);
                await _unitOfWork.CommitAsync();
            }
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
            var roleEntity = getRoleEntity(role);

            await _unitOfWork.RoleRepository.UpdateAsync(roleEntity);
            await _unitOfWork.CommitAsync();

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

        private Role getRoleEntity(DemoIdentityRole role)
        {
            var model = new Role();
            populateRoleEntity(model, role);
            return model;
        }

        private void populateRoleEntity(Role entity, DemoIdentityRole role)
        {
            entity.Id = getGuid(role.Id);
            entity.Name = role.Name;
            entity.NormalizedName = role.NormalizedName;
            entity.ConcurrencyStamp = role.ConcurrencyStamp;
        }

        private DemoIdentityRole getIdentityRole(Role roleEntity)
        {
            var role = new DemoIdentityRole();
            populateIdentityRole(role, roleEntity);
            return role;
        }

        private void populateIdentityRole(DemoIdentityRole role, Role roleEntity)
        {
            role.Id = roleEntity.Id.ToString();
            role.Name = roleEntity.Name;
            role.NormalizedName = roleEntity.NormalizedName;
            role.ConcurrencyStamp = roleEntity.ConcurrencyStamp;
        }
    }
}
