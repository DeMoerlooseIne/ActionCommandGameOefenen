using System.Collections.Generic;
using System.Threading.Tasks;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IPlayerItemService
    {
        Task<ServiceResult<PlayerItemResult>> GetAsync(int id, string authenticatedUserId);
        Task<ServiceResult<IList<PlayerItemResult>>> FindAsync(PlayerItemFilter filter, string authenticatedUserId);
        Task<ServiceResult<PlayerItemResult>> CreateAsync(int playerId, int itemId, string authenticatedUserId);
        Task<ServiceResult> DeleteAsync(int id, string authenticatedUserId);
    }
}
