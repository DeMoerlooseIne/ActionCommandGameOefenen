using ActionCommandGame.Api.Authentication.Extensions;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Api.Controllers
{
    public class PlayerItemsController : ApiBaseController
    {
        private readonly IPlayerItemService _playerItemService;

        public PlayerItemsController(IPlayerItemService playerItemService)
        {
            _playerItemService = playerItemService;
        }

        [HttpGet("player-items")]
        public async Task<IActionResult> Find([FromQuery] PlayerItemFilter filter)
        {
            var result = await _playerItemService.FindAsync(filter, User.GetId());
            return Ok(result);
        }
    }
}
