using CoreIdentityDemo.Services.Data.Entities;
using System.Data.Entity.ModelConfiguration;

namespace CoreIdentityDemo.Services.Data.Configuration
{
    public class RoleConfiguration : EntityTypeConfiguration<Role>
    {
        public RoleConfiguration()
        {
            ToTable("Roles");

            HasKey(x => x.Id);

            Property(x => x.Name)
                .HasMaxLength(256);

            Property(x => x.NormalizedName)
                .HasMaxLength(256);

            Property(x => x.ConcurrencyStamp)
                .IsConcurrencyToken();

            HasMany(x => x.Users)
                .WithMany(x => x.Roles)
                .Map(x =>
                {
                    x.ToTable("UserRoles");
                    x.MapLeftKey("RoleId");
                    x.MapRightKey("UserId");
                });

            HasMany(x => x.RoleClaims)
                .WithRequired(x => x.Role)
                .HasForeignKey(x => x.RoleId);
        }
    }
}