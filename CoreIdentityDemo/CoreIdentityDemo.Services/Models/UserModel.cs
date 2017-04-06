using System;
using System.Runtime.Serialization;

namespace CoreIdentityDemo.Services.Models
{
    [DataContract]
    public class UserModel
    {
        [DataMember(IsRequired = true)]
        public Guid Id { get; set; }

        [DataMember(IsRequired = true)]
        public string UserName { get; set; }

        [DataMember(IsRequired = true)]
        public string NormalizedUserName { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string NormalizedEmail { get; set; }

        [DataMember]
        public bool EmailConfirmed { get; set; }

        [DataMember]
        public string PasswordHash { get; set; }

        [DataMember]
        public string SecurityStamp { get; set; }

        [DataMember]
        public string ConcurrencyStamp { get; set; }

        [DataMember]
        public string PhoneNumber { get; set; }

        [DataMember]
        public bool PhoneNumberConfirmed { get; set; }

        [DataMember]
        public bool TwoFactorEnabled { get; set; }

        [DataMember]
        public DateTimeOffset? LockoutEnd { get; set; }

        [DataMember]
        public bool LockoutEnabled { get; set; }

        [DataMember]
        public int AccessFailedCount { get; set; }
    }
}
