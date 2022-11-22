using ActionCommandGame.Api.Installers.Abstractions;

namespace ActionCommandGame.Api.Installers
{
    public class ApiInstaller: IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
        }
    }
}
