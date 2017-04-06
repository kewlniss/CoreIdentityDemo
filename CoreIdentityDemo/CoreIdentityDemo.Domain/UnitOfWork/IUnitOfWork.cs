using CoreIdentityDemo.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace CoreIdentityDemo.Domain.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRoleClaimRepository RoleClaimRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserClaimRepository UserClaimRepository { get; }
        IUserLoginRepository UserLoginRepository { get; }
        IUserRepository UserRepository { get; }
        IUserTokenRepository UserTokenRepository { get; }

        Task CommitAsync();
        void Commit();
    }
}
