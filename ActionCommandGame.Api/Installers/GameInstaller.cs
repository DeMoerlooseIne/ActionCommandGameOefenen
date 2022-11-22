using ActionCommandGame.Api.Installers.Abstractions;
using ActionCommandGame.Services.Settings;

namespace ActionCommandGame.Api.Installers
{
    public class GameInstaller: IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            var gameSettings = new GameSettings();
            configuration.Bind(nameof(gameSettings), gameSettings);
            services.AddSingleton(gameSettings);
        }
    }
}
