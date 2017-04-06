using System;

namespace CoreIdentityDemo.Domain.Entities
{
    public class RoleClaim : Claim
    {
        public virtual Guid RoleId { get; set; }
    }
}