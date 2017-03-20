using CoreIdentityDemo.Services.Data.Entities;
using System.Data.Entity.ModelConfiguration;

namespace CoreIdentityDemo.Services.Data.Configuration
{
    public class UserLoginConfiguration : EntityTypeConfiguration<UserLogin>
    {
        public UserLoginConfiguration()
        {
            ToTable("UserLogins");

            HasKey(x => new { x.LoginProvider, x.ProviderKey });

            HasRequired(x => x.User)
                .WithMany(x => x.UserLogins)
                .HasForeignKey(x => x.UserId);
        }
    }
}