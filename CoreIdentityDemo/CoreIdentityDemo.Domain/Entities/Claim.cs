namespace CoreIdentityDemo.Domain.Entities
{
    public abstract class Claim
    {
        public virtual int Id { get; set; }
        public virtual string Type { get; set; }
        public virtual string Value { get; set; }
    }
}