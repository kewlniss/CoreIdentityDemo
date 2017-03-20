using System;

namespace CoreIdentityDemo.Web.Identity
{
    public class XIdentityRole
    {
        public XIdentityRole()
        {
            Id = Guid.NewGuid().ToString();
        }

        public XIdentityRole(string roleName)
            : this()
        {
            Name = roleName;
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
