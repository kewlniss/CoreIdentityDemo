﻿using System.Runtime.Serialization;

namespace CoreIdentityDemo.Common.IdentityApi
{
    [DataContract]
    public class ReplaceUserClaimArgs
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
