using System.Net.Http.Json;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Sdk.Extensions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Sdk
{
    public class PlayerItemApi: IPlayerItemApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenStore _tokenStore;

        public PlayerItemApi(IHttpClientFactory httpClientFactory, ITokenStore tokenStore)
        {
            _httpClientFactory = httpClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<ServiceResult<IList<PlayerItemResult>>> FindAsync(PlayerItemFilter filter)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGame");
            var token = await _tokenStore.GetTokenAsync();
            httpClient.AddAuthorization(token);
            var route = "player-items";

            if (filter.PlayerId.HasValue)
            {
                route += $"?playerId={filter.PlayerId}";
            }

            var httpResponse = await httpClient.GetAsync(route);

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<ServiceResult<IList<PlayerItemResult>>>();

            if (result is null)
            {
                return new ServiceResult<IList<PlayerItemResult>>();
            }

            return result;
        }
    }
}
