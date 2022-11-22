namespace ActionCommandGame.Sdk.Abstractions
{
    public interface ITokenStore
    {
        Task<string> GetTokenAsync();
        Task SaveTokenAsync(string token);
    }
}
