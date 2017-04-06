using CoreIdentityDemo.Domain.Entities;
using CoreIdentityDemo.Domain.Keys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreIdentityDemo.Domain.Repositories
{
    public interface IUserLoginRepository : IRepository<UserLogin, UserLoginKey>
    {
        IEnumerable<UserLogin> GetByUserId(Guid userId);
        Task<IEnumerable<UserLogin>> GetByUserIdAsync(Guid userId);
    }
}
