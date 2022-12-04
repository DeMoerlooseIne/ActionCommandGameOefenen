using ActionCommandGame.Ui.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Services.Model.Requests;
using ActionCommandGame.Services.Model.Results;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;


namespace ActionCommandGame.Ui.WebApp.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private readonly IIdentityApi _identityApi;
        private readonly ITokenStore _tokenStore;
        private readonly IPlayerStore _playerStore;
        private readonly IPlayerApi _playerApi;
        private readonly IItemApi _itemApi;
        private readonly IGameApi _gameApi;
        private readonly IPlayerItemApi _playerItemApi;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string AuthSchemes = CookieAuthenticationDefaults.AuthenticationScheme;

        public GameController(IIdentityApi identityApi,
                                ITokenStore tokenStore,
                                IPlayerStore playerStore,
                                IPlayerApi playerApi,
                                IItemApi itemApi,
                                IGameApi gameApi,
                                IPlayerItemApi playerItemApi,
                                IHttpContextAccessor httpContextAccessor)
        {
            _identityApi = identityApi;
            _playerApi = playerApi;
            _itemApi = itemApi;
            _tokenStore = tokenStore;
            _playerStore = playerStore;
            _gameApi = gameApi;
            _playerItemApi = playerItemApi;
            _httpContextAccessor = httpContextAccessor;

        }




        public async Task<IActionResult> Index()
        {

            var playerAction = await CreatePlayerAction();
            if (playerAction is null)
            {
                return RedirectToAction("PickPlayer");
            }
            return View(playerAction);
        }

        [Route("/shop")]
        public async Task<IActionResult> Shop()
        {
            var playerAction = await CreatePlayerAction();
            if (playerAction is null)
            {
                return RedirectToAction("PickPlayer");
            }
            var itemsRequest = await _itemApi.FindAsync();
            if (!itemsRequest.IsSuccess)
            {
                return RedirectToAction("PickPlayer");
            }
            return View(new PlayerShop
            {
                Items = itemsRequest.Data,
                Id=playerAction.Id,
                Player = playerAction.Player

            });
        }
        public async Task<IActionResult> Buy(ItemResult shopItem)
        {
            var playerId = await _playerStore.GetTokenAsync();
            var buyResult = await _gameApi.BuyAsync(playerId, shopItem.Id);
            return RedirectToAction("index");
        }
        public async Task<IActionResult> PerformAction()
        {
            var playerAction = await CreatePlayerAction();
            if (playerAction is null)
            {
                return RedirectToAction("PickPlayer");
            }
            var gameResult = await _gameApi.PerformActionAsync(playerAction.Id);
            if (!gameResult.IsSuccess || gameResult.Data is null)
            {
                return RedirectToAction("index");
            }

            playerAction.GameResult = gameResult.Data;
            
            playerAction.Messages = gameResult.Messages;

            return View("Index", playerAction);



        }

        public async Task<IActionResult> Inventory()
        {
            var playerAction = await CreatePlayerAction();
            if (playerAction is null)
            {
                return RedirectToAction("PickPlayer");
            }

            var inventory = await _playerItemApi.FindAsync(new PlayerItemFilter
            {
                PlayerId = playerAction.Id
            });
            playerAction.Items = inventory.Data;
            return View(playerAction);
        }

        public async Task<IActionResult> LeaderBoard()
        {
            var user = await GetUser();
            return View(user);
        }
        [HttpGet]
        public async Task<IActionResult> PickPlayer()
        {
            var user = await GetUser();
            if (user.Players is null || user.Players.Count is 0)
            {
                return RedirectToAction("CreatePlayer");
            }
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> PickPlayer([FromRoute] int id)
        {
            await _playerStore.SaveTokenAsync(id);
            return RedirectToAction("index");

        }

  
        [HttpGet]
        public IActionResult CreatePlayer()
        {
            return View(new CreatePlayerRequest());
        }
        [HttpPost]
        public async Task<IActionResult> CreatePlayer([FromForm] CreatePlayerRequest player)
        {
            var createPlayer = await _playerApi.CreatePlayer(player);
            return RedirectToAction("PickPlayer");

        }

        private async Task<PlayerAction?> CreatePlayerAction()
        {
            var playerId = await _playerStore.GetTokenAsync();
            if (playerId < 0)
            {
                return null;
            }
            var player = await _playerApi.GetAsync(playerId);
            if (!player.IsSuccess || player.Data is null)
            {
                return null;
            }

            return new PlayerAction
            {
                Player = player.Data,
                Id = playerId
            };
        }

        private async Task<User> GetUser()
        {
            var playersResult = await _playerApi.Find(new PlayerFilter
            {
                FilterUserPlayers = true
            });

            if (_httpContextAccessor.HttpContext is null || !playersResult.IsSuccess)
            {
                return null;
            }


            var claimsIdentity = User.Identity as ClaimsIdentity;
            var emailClaim = claimsIdentity.FindFirst(ClaimTypes.Email);
            var username = emailClaim.Value;

            return new User
            {
                Players = playersResult.Data,
                UserName = username

            };
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}