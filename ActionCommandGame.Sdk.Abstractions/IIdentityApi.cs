using ActionCommandGame.Api.Authentication.Model;

namespace ActionCommandGame.Sdk.Abstractions
{
    public interface IIdentityApi
    {
        Task<AuthenticationResult> SignInAsync(UserSignInRequest request);
        Task<AuthenticationResult> RegisterAsync(UserRegistrationRequest request);
    } 
}
