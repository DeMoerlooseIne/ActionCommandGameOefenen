using System.Security.Claims;

namespace ActionCommandGame.Api.Authentication.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static T? GetId<T>(this ClaimsPrincipal principal)
        {
            return principal.GetClaimValue<T>("Id");
        }

        public static string? GetId(this ClaimsPrincipal principal)
        {
            return principal.GetClaimValue<string>("Id");
        }

        private static T? GetClaimValue<T>(this ClaimsPrincipal principal, string name)
        {
            var identity = principal.Identity as ClaimsIdentity;
            if (identity is null)
            {
                return default;
            }
            return identity.GetClaimValue<T>(name);
        }
    }
}
