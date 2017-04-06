using System.Runtime.Serialization;

namespace CoreIdentityDemo.Services.Models
{
    [DataContract]
    public class LoginModel
    {
        [DataMember(IsRequired = true)]
        public string LoginProvider { get; set; }

        [DataMember(IsRequired = true)]
        public string ProviderKey { get; set; }

        [DataMember(IsRequired = true)]
        public string ProviderDisplayName { get; set; }
    }
}
