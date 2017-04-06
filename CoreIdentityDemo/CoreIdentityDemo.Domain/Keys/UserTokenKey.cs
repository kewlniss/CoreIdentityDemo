using System;

namespace CoreIdentityDemo.Domain.Keys
{
    public class UserTokenKey
    {
        public Guid UserId { get; set; }
        public string LoginProvider { get; set; }
        public string Name { get; set; }
    }
}
