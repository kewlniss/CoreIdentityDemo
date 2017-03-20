using System;

namespace CoreIdentityDemo.Services.Data.Entities
{
    public class RoleClaim : Claim
    {
        public virtual Guid RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}