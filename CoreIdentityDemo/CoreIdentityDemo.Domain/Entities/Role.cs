using System;

namespace CoreIdentityDemo.Domain.Entities
{
    public class Role
    {
        public virtual Guid Id { get; set; }
        public virtual string Name { get; set; }
        public virtual string NormalizedName { get; set; }
        public virtual string ConcurrencyStamp { get; set; }
    }
}