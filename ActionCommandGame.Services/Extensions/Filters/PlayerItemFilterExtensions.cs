using System.Linq;
using ActionCommandGame.Model;
using ActionCommandGame.Services.Model.Filters;

namespace ActionCommandGame.Services.Extensions.Filters
{
    internal static class PlayerItemFilterExtensions
    {
        public static IQueryable<PlayerItem> ApplyFilter(this IQueryable<PlayerItem> query,
            PlayerItemFilter filter)
        {
            if (filter is null)
            {
                return query;
            }

            if (filter.PlayerId.HasValue)
            {
                query = query.Where(pi => pi.PlayerId == filter.PlayerId.Value);
            }

            return query;
        }
    }
}
