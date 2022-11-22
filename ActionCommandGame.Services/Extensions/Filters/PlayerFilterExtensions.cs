using System.Linq;
using ActionCommandGame.Model;
using ActionCommandGame.Services.Model.Filters;

namespace ActionCommandGame.Services.Extensions.Filters
{
    internal static class PlayerFilterExtensions
    {
        public static IQueryable<Player> ApplyFilter(this IQueryable<Player> query, PlayerFilter filter, string authenticatedUserId)
        {
            if (filter is null)
            {
                return query;
            }

            if (filter.FilterUserPlayers.HasValue && filter.FilterUserPlayers.Value)
            {
                query = query.Where(p => p.UserId == authenticatedUserId);
            }

            return query;
        }
    }
}
