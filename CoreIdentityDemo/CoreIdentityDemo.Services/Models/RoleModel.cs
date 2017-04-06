using System;
using System.Runtime.Serialization;

namespace CoreIdentityDemo.Services.Models
{
    [DataContract]
    public class RoleModel
    {
        [DataMember(IsRequired = true)]
        public Guid Id { get; set; }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [DataMember]
        public string NormalizedName { get; set; }

        [DataMember]
        public string ConcurrencyStamp { get; set; }
    }
}
