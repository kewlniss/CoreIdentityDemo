﻿using System.Runtime.Serialization;

namespace CoreIdentityDemo.Common.IdentityApi
{
    [DataContract]
    public class TokenModel
    {
        [DataMember(IsRequired = true)]
        public string LoginProvider { get; set; }

        [DataMember(IsRequired = true)]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }
    }
}
