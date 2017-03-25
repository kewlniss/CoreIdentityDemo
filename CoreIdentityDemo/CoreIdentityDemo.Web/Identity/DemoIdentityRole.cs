using System;

namespace CoreIdentityDemo.Web.Identity
{
    public class DemoIdentityRole
    {
        public DemoIdentityRole()
        {
            Id = Guid.NewGuid().ToString();
        }

        public DemoIdentityRole(string roleName)
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
