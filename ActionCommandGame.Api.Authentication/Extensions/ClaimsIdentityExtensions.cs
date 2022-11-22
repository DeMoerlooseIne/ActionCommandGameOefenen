using System.ComponentModel;
using System.Security.Claims;

namespace ActionCommandGame.Api.Authentication.Extensions
{
    public static class ClaimsIdentityExtensions
    {
        public static T? GetClaimValue<T>(this ClaimsIdentity? identity, string name)
        {
            if (identity is null)
            {
                return default;
            }

            var claim = identity.FindFirst(name);

            if (claim?.Value is null)
            {
                return default;
            }

            var value = (T?)TypeDescriptor.GetConverter(typeof(T)).ConvertFromString(claim.Value);

            return value;
        }

        public static void SetClaimValue<T>(this ClaimsIdentity? identity, string name, T value)
        {
            if (identity is null)
            {
                return;
            }

            var claim = identity.FindFirst(name);

            if (claim is null)
            {
                identity.RemoveClaim(claim);
            }

            // add new claim
            identity?.AddClaim(new Claim(name, value.ToString() ?? string.Empty));
        }
	}
}
