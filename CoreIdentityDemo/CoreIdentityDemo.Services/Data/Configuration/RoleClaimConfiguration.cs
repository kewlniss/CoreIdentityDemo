using CoreIdentityDemo.Services.Data.Entities;
using System.Data.Entity.ModelConfiguration;

namespace CoreIdentityDemo.Services.Data.Configuration
{
    public class RoleClaimConfiguration : EntityTypeConfiguration<RoleClaim>
    {
        public RoleClaimConfiguration()
        {
            ToTable("RoleClaims");

            HasRequired(x => x.Role)
                .WithMany(x => x.RoleClaims)
                .HasForeignKey(x => x.RoleId);
        }
    }
}