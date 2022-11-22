using System.Collections.Generic;
using System.Threading.Tasks;
using ActionCommandGame.Model;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IItemService
    {
        Task<ServiceResult<ItemResult>> GetAsync(int id, string authenticatedUserId);
        Task<ServiceResult<IList<ItemResult>>> FindAsync(string authenticatedUserId);
    }
}
