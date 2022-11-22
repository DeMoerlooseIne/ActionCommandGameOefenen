using ActionCommandGame.Api.Authentication.Extensions;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Model.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Api.Controllers
{
    public class PlayersController : ApiBaseController
    {
        private readonly IPlayerService _playerService;

        public PlayersController(IPlayerService playerService)
        {
            _playerService = playerService;
        }

        [HttpGet("players/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _playerService.GetAsync(id, User.GetId());
            return Ok(result);
        }

        [HttpGet("players")]
        public async Task<IActionResult> Find([FromQuery]PlayerFilter filter)
        {
            var result = await _playerService.FindAsync(filter, User.GetId());
            return Ok(result);
        }
    }
}
