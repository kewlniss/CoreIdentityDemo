using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CoreIdentityDemo.Services.Data.Entities
{
    public class UserClaim : Claim
    {
        public virtual Guid UserId { get; set; }
        public virtual User User { get; set; }
    }
}