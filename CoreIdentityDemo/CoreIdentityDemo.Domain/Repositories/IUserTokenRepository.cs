using CoreIdentityDemo.Domain.Entities;
using CoreIdentityDemo.Domain.Keys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreIdentityDemo.Domain.Repositories
{
    public interface IUserTokenRepository : IRepository<UserToken, UserTokenKey>
    {
        IEnumerable<UserToken> GetByUserId(Guid userId);
        Task<IEnumerable<UserToken>> GetByUserIdAsync(Guid userId);
    }
}
