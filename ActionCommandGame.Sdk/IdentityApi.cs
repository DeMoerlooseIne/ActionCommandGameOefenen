using System.Net.Http.Json;
using ActionCommandGame.Api.Authentication.Model;
using ActionCommandGame.Sdk.Abstractions;

namespace ActionCommandGame.Sdk
{
    public class IdentityApi: IIdentityApi
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenStore _tokenStore;

        public IdentityApi(IHttpClientFactory httpClientFactory, ITokenStore tokenStore)
        {
            _httpClientFactory = httpClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<AuthenticationResult> SignInAsync(UserSignInRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGame");
            var route = "identity/sign-in";

            var httpResponse = await httpClient.PostAsJsonAsync(route, request);

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<AuthenticationResult>();

            if (result is null)
            {
                return new AuthenticationResult();
            }

            return result;
        }

        public async Task<AuthenticationResult> RegisterAsync(UserRegistrationRequest request)
        {
            var httpClient = _httpClientFactory.CreateClient("ActionCommandGame");
            var route = "identity/register";

            var httpResponse = await httpClient.PostAsJsonAsync(route, request);

            httpResponse.EnsureSuccessStatusCode();

            var result = await httpResponse.Content.ReadFromJsonAsync<AuthenticationResult>();

            if (result is null)
            {
                return new AuthenticationResult();
            }

            return result;
        }
    }
}
