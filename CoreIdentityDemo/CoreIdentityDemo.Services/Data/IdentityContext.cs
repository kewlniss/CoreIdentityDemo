using CoreIdentityDemo.Services.Data.Configuration;
using CoreIdentityDemo.Services.Data.Entities;
using System.Data.Entity;

namespace CoreIdentityDemo.Services.Data
{
    public class IdentityContext : DbContext
    {
        public IdentityContext()
            : this("name=CoreIdentityDemo")
        { }

        public IdentityContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        { }

        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserClaim> UserClaims { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new ClaimConfiguration());
            modelBuilder.Configurations.Add(new RoleConfiguration());
            modelBuilder.Configurations.Add(new RoleClaimConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new UserClaimConfiguration());
            modelBuilder.Configurations.Add(new UserLoginConfiguration());
            modelBuilder.Configurations.Add(new UserTokenConfiguration());
        }
    }
}