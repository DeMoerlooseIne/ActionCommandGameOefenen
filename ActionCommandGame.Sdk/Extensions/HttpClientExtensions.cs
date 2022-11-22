using System.Net.Http.Headers;

namespace ActionCommandGame.Sdk.Extensions
{
    internal static class HttpClientExtensions
    {
        public static void AddAuthorization(this HttpClient httpClient, string bearerToken)
        {
            httpClient.DefaultRequestHeaders.AddAuthorization(bearerToken);
        }

        public static void AddAuthorization(this HttpRequestHeaders httpRequestHeaders, string bearerToken)
        {
            if (httpRequestHeaders.Contains("Authorization"))
            {
                httpRequestHeaders.Remove("Authorization");
            }
            httpRequestHeaders.Add("Authorization", $"Bearer {bearerToken}");
        }
	}
}
