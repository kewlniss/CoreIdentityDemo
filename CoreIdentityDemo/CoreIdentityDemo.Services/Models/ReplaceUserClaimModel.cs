using System.Runtime.Serialization;

namespace CoreIdentityDemo.Services.Models
{
    [DataContract]
    public class ReplaceUserClaimModel
    {
        [DataMember(IsRequired = true)]
        public string claimType { get; set; }

        [DataMember]
        public string claimValue { get; set; }

        [DataMember(IsRequired = true)]
        public string newClaimType { get; set; }

        [DataMember]
        public string newClaimValue { get; set; }
    }
}
