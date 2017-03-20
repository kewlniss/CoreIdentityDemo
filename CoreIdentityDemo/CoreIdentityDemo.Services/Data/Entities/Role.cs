using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreIdentityDemo.Services.Data.Entities
{
    public class Role
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }

        [Index("RoleNameIndex", IsUnique = true)]
        public virtual string NormalizedName { get; set; }

        public virtual string ConcurrencyStamp { get; set; }

        public virtual ICollection<RoleClaim> RoleClaims { get; set; } = new List<RoleClaim>();
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}