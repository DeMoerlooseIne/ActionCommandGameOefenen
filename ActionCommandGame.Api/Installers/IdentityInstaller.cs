using ActionCommandGame.Api.Authentication;
using ActionCommandGame.Api.Authentication.Abstractions;
using ActionCommandGame.Api.Installers.Abstractions;
using ActionCommandGame.Repository;
using Microsoft.AspNetCore.Identity;

namespace ActionCommandGame.Api.Installers
{
    public class IdentityInstaller: IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
            }).AddEntityFrameworkStores<ActionCommandGameDbContext>();

            services.AddTransient<IIdentityService, IdentityService>();
        }
    }
}
