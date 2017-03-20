using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CoreIdentityDemo.Services.Data.Entities
{
    public abstract class Claim
    {
        public virtual int Id { get; set; }
        public virtual string Type { get; set; }
        public virtual string Value { get; set; }
    }
}