using CoreIdentityDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreIdentityDemo.Domain.Repositories
{
    public interface IRoleRepository : IRepository<Role, Guid>
    {
        Role GetByNormalizedName(string normalizedName);
        Task<Role> GetByNormalizedNameAsync(string normalizedName);

        IEnumerable<Role> GetByUserId(Guid userId);
        Task<IEnumerable<Role>> GetByUserIdAsync(Guid userId);
    }
}
