using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CoreIdentityDemo.Web.Identity
{
    public static class BuilderExtensions
    {
        public static IdentityBuilder AddCustomStores(this IdentityBuilder builder)
        {
            builder.Services.AddScoped<IUserStore<XIdentityUser>, XUserStore>();
            builder.Services.AddScoped<IRoleStore<XIdentityRole>, XRoleStore>();
            return builder;
        }
    }
}
