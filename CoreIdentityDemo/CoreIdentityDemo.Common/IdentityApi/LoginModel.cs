using System.ComponentModel.DataAnnotations;

namespace CoreIdentityDemo.Common.IdentityApi
{
    public class LoginModel
    {
        [Required]
        public string LoginProvider { get; set; }
        [Required]
        public string ProviderKey { get; set; }
        public string ProviderDisplayName { get; set; }
    }
}
