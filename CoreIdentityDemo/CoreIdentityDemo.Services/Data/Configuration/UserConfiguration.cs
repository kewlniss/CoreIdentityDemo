using CoreIdentityDemo.Services.Data.Entities;
using System.Data.Entity.ModelConfiguration;

namespace CoreIdentityDemo.Services.Data.Configuration
{
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        public UserConfiguration()
        {
            ToTable("Users");
            HasKey(x => x.Id);
            Property(x => x.UserName).HasMaxLength(256);
            Property(x => x.NormalizedUserName).HasMaxLength(256);
            Property(x => x.Email).HasMaxLength(256);
            Property(x => x.NormalizedEmail).HasMaxLength(256);
            Property(x => x.ConcurrencyStamp).IsConcurrencyToken();

            HasMany(x => x.UserClaims)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.UserId);

            HasMany(x => x.Roles)
                .WithMany(x => x.Users)
                .Map(x =>
                {
                    x.ToTable("UserRoles");
                    x.MapLeftKey("UserId");
                    x.MapRightKey("RoleId");
                });

            HasMany(x => x.UserLogins)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.UserId);

            HasMany(x => x.UserTokens)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.UserId);
        }
    }
}