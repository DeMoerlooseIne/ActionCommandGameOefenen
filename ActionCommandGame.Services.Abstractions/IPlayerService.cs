using System.Collections.Generic;
using System.Threading.Tasks;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IPlayerService
    {
        Task<ServiceResult<PlayerResult>> GetAsync(int id, string authenticatedUserId);
        Task<ServiceResult<IList<PlayerResult>>> FindAsync(PlayerFilter filter, string authenticatedUserId);
    }
}
