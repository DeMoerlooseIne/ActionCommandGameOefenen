using System.ComponentModel.DataAnnotations;

namespace ActionCommandGame.Api.Authentication.Model
{
	public class UserSignInRequest
	{
		public string? Email { get; set; }

		public string? Password { get; set; }
	}
}
