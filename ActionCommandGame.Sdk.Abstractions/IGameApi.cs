using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Sdk.Abstractions
{
    public interface IGameApi
    {
        Task<ServiceResult<GameResult>> PerformActionAsync(int playerId);
        Task<ServiceResult<BuyResult>> BuyAsync(int playerId, int itemId);
    }
}
