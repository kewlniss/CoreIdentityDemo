using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CoreIdentityDemo.Web.Identity
{
    public static class BuilderExtensions
    {
        public static IdentityBuilder AddCustomStores(this IdentityBuilder builder)
        {
            builder.Services.AddScoped<IUserStore<DemoIdentityUser>, DemoUserStore>();
            builder.Services.AddScoped<IRoleStore<DemoIdentityRole>, DemoRoleStore>();
            return builder;
        }
    }
}
