using CoreIdentityDemo.Services.Data.Entities;
using System.Data.Entity.ModelConfiguration;

namespace CoreIdentityDemo.Services.Data.Configuration
{
    public class UserTokenConfiguration : EntityTypeConfiguration<UserToken>
    {
        public UserTokenConfiguration()
        {
            ToTable("UserTokens");

            HasKey(x => new { x.UserId, x.LoginProvider, x.Name });

            HasRequired(x => x.User)
                .WithMany(x => x.UserTokens)
                .HasForeignKey(x => x.UserId);
        }
    }
}