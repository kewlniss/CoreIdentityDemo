using CoreIdentityDemo.Services.Data.Entities;
using System.Data.Entity.ModelConfiguration;

namespace CoreIdentityDemo.Services.Data.Configuration
{
    public class UserClaimConfiguration : EntityTypeConfiguration<UserClaim>
    {
        public UserClaimConfiguration()
        {
            ToTable("UserClaims");

            HasRequired(x => x.User)
                .WithMany(x => x.UserClaims)
                .HasForeignKey(x => x.UserId);
        }
    }
}