using ActionCommandGame.Api.Authentication.Abstractions;
using ActionCommandGame.Api.Authentication.Model;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Api.Controllers
{
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("identity/sign-in")]
        public async Task<IActionResult> SignIn(UserSignInRequest request)
        {
            var authenticationResult = await _identityService.SignInAsync(request);
            return Ok(authenticationResult);
        }

        [HttpPost("identity/register")]
        public async Task<IActionResult> Register(UserRegistrationRequest request)
        {
            var authenticationResult = await _identityService.RegisterAsync(request);
            return Ok(authenticationResult);
        }
    }
}
