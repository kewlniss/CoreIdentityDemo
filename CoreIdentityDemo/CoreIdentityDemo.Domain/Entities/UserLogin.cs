using System;

namespace CoreIdentityDemo.Domain.Entities
{
    public class UserLogin
    {
        public virtual string LoginProvider { get; set; }
        public virtual string ProviderKey { get; set; }
        public virtual string ProviderDisplayName { get; set; }
        public virtual Guid UserId { get; set; }
    }
}