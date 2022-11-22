using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Sdk.Abstractions
{
    public interface IPlayerItemApi
    {
        Task<ServiceResult<IList<PlayerItemResult>>> FindAsync(PlayerItemFilter filter);
    }
}
