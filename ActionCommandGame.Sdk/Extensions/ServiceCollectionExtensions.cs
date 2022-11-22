using ActionCommandGame.Sdk.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace ActionCommandGame.Sdk.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApi(this IServiceCollection services, string baseUrl)
    {
        services.AddApi(new Uri(baseUrl));
    }

    public static void AddApi(this IServiceCollection services, Uri baseUri)
    {
        services.AddHttpClient("ActionCommandGame", (sp, c) =>
            {
                c.BaseAddress = baseUri;
            });

        services.AddTransient<IGameApi, GameApi>();
        services.AddTransient<IPlayerApi, PlayerApi>();
        services.AddTransient<IItemApi, ItemApi>();
        services.AddTransient<IPlayerItemApi, PlayerItemApi>();
        services.AddTransient<IIdentityApi, IdentityApi>();
    }
}