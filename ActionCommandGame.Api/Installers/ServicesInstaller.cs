using ActionCommandGame.Api.Installers.Abstractions;
using ActionCommandGame.Services;
using ActionCommandGame.Services.Abstractions;

namespace ActionCommandGame.Api.Installers
{
    public class ServicesInstaller: IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IGameService, GameService>();
            services.AddTransient<IPositiveGameEventService, PositiveGameEventService>();
            services.AddTransient<INegativeGameEventService, NegativeGameEventService>();
            services.AddTransient<IItemService, ItemService>();
            services.AddTransient<IPlayerService, PlayerService>();
            services.AddTransient<IPlayerItemService, PlayerItemService>();
        }
    }
}
