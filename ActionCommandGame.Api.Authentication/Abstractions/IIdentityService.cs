using ActionCommandGame.Api.Authentication.Model;

namespace ActionCommandGame.Api.Authentication.Abstractions
{
	public interface IIdentityService
	{
		Task<AuthenticationResult> RegisterAsync(UserRegistrationRequest request);
		Task<AuthenticationResult> SignInAsync(UserSignInRequest request);
	}
}
