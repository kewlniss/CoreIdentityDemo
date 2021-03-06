﻿using System.Runtime.Serialization;

namespace CoreIdentityDemo.Common.IdentityApi
{
    [DataContract]
    public class ClaimModel
    {
        [DataMember]
        public string Type { get; set; }

        [DataMember]
        public string Value { get; set; }
    }
}
