using System.Threading.Tasks;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Abstractions
{
    public interface IPositiveGameEventService
    {
        Task<ServiceResult<PositiveGameEventResult>> GetRandomPositiveGameEvent(bool hasAttackItem, string authenticatedUserId);
    }
}
