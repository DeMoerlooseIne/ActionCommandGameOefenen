using System.Net.Http.Json;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Sdk.Extensions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Sdk
{
    public class PlayerApi: IPlayerApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenStore _tokenStore;

        public PlayerApi(IHttpClientFactory httpClientFactory, ITokenStore tokenStore)
        {
            _httpClientFactory = httpClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<ServiceResult<PlayerResult>> GetAsync(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGame");
            var token = await _tokenStore.GetTokenAsync();
            httpClient.AddAuthorization(token);
            var route = $"players/{id}";

            var httpResponse = await httpClient.GetAsync(route);

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<ServiceResult<PlayerResult>>();

            if (result is null)
            {
                return new ServiceResult<PlayerResult>();
            }

            return result;
        }

        public async Task<ServiceResult<IList<PlayerResult>>> Find(PlayerFilter filter)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGame");
            var token = await _tokenStore.GetTokenAsync();
            httpClient.AddAuthorization(token);
            var route = "players";

            if (filter.FilterUserPlayers.HasValue && filter.FilterUserPlayers.Value)
            {
                route += $"?FilterUserPlayers={filter.FilterUserPlayers}";
            }

            var httpResponse = await httpClient.GetAsync(route);

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<ServiceResult<IList<PlayerResult>>>();

            if (result is null)
            {
                return new ServiceResult<IList<PlayerResult>>();
            }

            return result;
        }
    }
}
