using System.ComponentModel.DataAnnotations;

namespace CoreIdentityDemo.Common.IdentityApi
{
    public class ReplaceUserClaimArgs
    {
        [Required]
        public string claimType { get; set; }
        public string claimValue { get; set; }
        [Required]
        public string newClaimType { get; set; }
        public string newClaimValue { get; set; }
    }
}
