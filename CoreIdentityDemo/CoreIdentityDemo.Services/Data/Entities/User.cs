using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreIdentityDemo.Services.Data.Entities
{
    public class User
    {
        public virtual Guid Id { get; set; }
        public virtual string UserName { get; set; }

        [Index("UserNameIndex", IsUnique = true)]
        public virtual string NormalizedUserName { get; set; }

        public virtual string Email { get; set; }

        [Index("EmailIndex", IsUnique = true)]
        public virtual string NormalizedEmail { get; set; }

        public virtual bool EmailConfirmed { get; set; }
        public virtual string PasswordHash { get; set; }
        public virtual string SecurityStamp { get; set; }
        public virtual string ConcurrencyStamp { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual bool PhoneNumberConfirmed { get; set; }
        public virtual bool TwoFactorEnabled { get; set; }
        public virtual DateTimeOffset? LockoutEnd { get; set; }
        public virtual bool LockoutEnabled { get; set; }
        public virtual int AccessFailedCount { get; set; }

        public virtual ICollection<UserClaim> UserClaims { get; set; } = new List<UserClaim>();
        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
        public virtual ICollection<UserLogin> UserLogins { get; set; } = new List<UserLogin>();
        public virtual ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();
    }
}