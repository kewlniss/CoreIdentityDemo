using System;

namespace CoreIdentityDemo.Services.Data.Entities
{
    public class UserToken
    {
        public virtual Guid UserId { get; set; }
        public virtual string LoginProvider { get; set; }
        public virtual string Name { get; set; }
        public virtual string Value { get; set; }

        public virtual User User { get; set; }
    }
}