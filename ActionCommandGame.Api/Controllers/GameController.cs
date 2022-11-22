using ActionCommandGame.Api.Authentication.Extensions;
using ActionCommandGame.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Api.Controllers
{
    public class GameController : ApiBaseController
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost("game/{playerId}/perform-action")]
        public async Task<IActionResult> PerformAction(int playerId)
        {
            var result = await _gameService.PerformActionAsync(playerId, User.GetId());
            return Ok(result);
        }

        [HttpPost("game/{playerId}/buy/{itemId}")]
        public async Task<IActionResult> Buy(int playerId, int itemId)
        {
            var result = await _gameService.BuyAsync(playerId, itemId, User.GetId());
            return Ok(result);
        }
    }
}
