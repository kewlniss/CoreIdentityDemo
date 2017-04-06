using System;

namespace CoreIdentityDemo.Domain.Entities
{
    public class UserClaim : Claim
    {
        public virtual Guid UserId { get; set; }
    }
}