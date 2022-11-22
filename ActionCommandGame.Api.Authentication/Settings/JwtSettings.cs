namespace ActionCommandGame.Api.Authentication.Settings
{
	public class JwtSettings
	{
		public string? Secret { get; set; }
		public TimeSpan TokenLifetime { get; set; }
	}
}
