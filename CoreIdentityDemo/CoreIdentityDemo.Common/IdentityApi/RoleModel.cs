using System;
using System.ComponentModel.DataAnnotations;

namespace CoreIdentityDemo.Common.IdentityApi
{
    public class RoleModel
    {
        [Required]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string ConcurrencyStamp { get; set; }
    }
}
