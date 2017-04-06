using CoreIdentityDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreIdentityDemo.Domain.Repositories
{
    public interface IUserClaimRepository : IRepository<UserClaim, int>
    {
        IEnumerable<UserClaim> GetByUserId(Guid userId);
        Task<IEnumerable<UserClaim>> GetByUserIdAsync(Guid userId);
    }
}
