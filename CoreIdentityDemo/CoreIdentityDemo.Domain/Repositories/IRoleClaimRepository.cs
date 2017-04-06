using CoreIdentityDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreIdentityDemo.Domain.Repositories
{
    public interface IRoleClaimRepository : IRepository<RoleClaim, int>
    {
        IEnumerable<RoleClaim> GetByRoleId(Guid roleId);
        Task<IEnumerable<RoleClaim>> GetByRoleIdAsync(Guid roleId);
    }
}
