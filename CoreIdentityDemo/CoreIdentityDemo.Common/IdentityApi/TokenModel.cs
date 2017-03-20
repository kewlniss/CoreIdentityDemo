using System.ComponentModel.DataAnnotations;

namespace CoreIdentityDemo.Common.IdentityApi
{
    public class TokenModel
    {
        [Required]
        public string LoginProvider { get; set; }
        [Required]
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
