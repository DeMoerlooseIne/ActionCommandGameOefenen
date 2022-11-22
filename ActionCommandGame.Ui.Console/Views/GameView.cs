using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionCommandGame.Extensions;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Results;
using ActionCommandGame.Ui.ConsoleApp.Abstractions;
using ActionCommandGame.Ui.ConsoleApp.ConsoleWriters;
using ActionCommandGame.Ui.ConsoleApp.Navigation;
using ActionCommandGame.Ui.ConsoleApp.Settings;
using ActionCommandGame.Ui.ConsoleApp.Stores;

namespace ActionCommandGame.Ui.ConsoleApp.Views
{
    internal class GameView : IView
    {
        private readonly AppSettings _settings;
        private readonly MemoryStore _memoryStore;
        private readonly NavigationManager _navigationManager;
        private readonly IGameApi _gameApi;
        private readonly IPlayerApi _playerApi;
        private readonly IItemApi _itemApi;

        public GameView(
            AppSettings settings,
            MemoryStore memoryStore,
            NavigationManager navigationManager,
            IGameApi gameApi,
            IPlayerApi playerApi,
            IItemApi itemApi,
            IPlayerItemApi playerItemApi)
        {
            _settings = settings;
            _memoryStore = memoryStore;
            _navigationManager = navigationManager;
            _gameApi = gameApi;
            _playerApi = playerApi;
            _itemApi = itemApi;
        }

        public async Task Show()
        {
            ConsoleWriter.WriteText($"Play your game. Try typing \"help\" or \"{_settings.ActionCommand}\"", ConsoleColor.Yellow);

            //Get the player from somewhere
            var currentPlayerId = _memoryStore.CurrentPlayerId;

            while (true)
            {

                ConsoleWriter.WriteText($"{_settings.CommandPromptText} ", ConsoleColor.DarkGray, false);

                string command = Console.ReadLine();

                Console.Clear();

                if (string.IsNullOrWhiteSpace(command))
                {
                    continue;
                }

                if (CheckCommand(command, new[] { "change player", "player", "change" }))
                {
                    await _navigationManager.NavigateTo<PlayerSelectionView>();
                    break;
                }

                if (CheckCommand(command, new[] { "exit", "quit", "stop" }))
                {
                    await _navigationManager.NavigateTo<ExitView>();
                    break;
                }

                if (CheckCommand(command, new[] { _settings.ActionCommand }))
                {
                    await PerformAction(currentPlayerId);

                    await ShowStats(currentPlayerId);
                }

                if (CheckCommand(command, new[] { "shop", "store" }))
                {
                    await _navigationManager.NavigateTo<ShopView>();
                }

                if (CheckCommand(command, new[] { "buy", "purchase", "get" }))
                {
                    var itemId = GetIdParameterFromCommand(command);

                    if (!itemId.HasValue)
                    {
                        ConsoleWriter.WriteText("I have no idea what you mean. I have tagged every item with a number. Please give me that number.", ConsoleColor.Red);
                        continue;
                    }

                    await Buy(currentPlayerId, itemId.Value);
                }

                if (CheckCommand(command, new[] { "bal", "balance", "money", "xp", "level", "statistics", "stats", "stat", "info" }))
                {
                    await ShowStats(currentPlayerId);
                }

                if (CheckCommand(command, new[] { "leaderboard", "lead", "top", "rank", "ranking" }))
                {
                    await _navigationManager.NavigateTo<LeaderboardView>();
                }

                if (CheckCommand(command, new[] { "inventory", "inv", "bag", "backpack" }))
                {
                    await _navigationManager.NavigateTo<InventoryView>();
                }

                if (CheckCommand(command, new[] { "?", "help", "h", "commands" }))
                {
                    await _navigationManager.NavigateTo<HelpView>();
                }
            }
        }



        private static bool CheckCommand(string command, IList<string> matchingCommands)
        {
            return matchingCommands.Any(c => command.ToLower().StartsWith(c.ToLower()));
        }

        public async Task ShowStats(int playerId)
        {
            var playerResult = await _playerApi.GetAsync(playerId);
            var player = playerResult.Data;

            //Check food consumption
            if (player.CurrentFuelId != null)
            {
                ConsoleWriter.WriteText($"[{player.CurrentFuelName}] ", ConsoleColor.Yellow, false);
                ConsoleWriter.WriteText($"{player.RemainingFuel}/{player.TotalFuel}  ", null, false);
            }
            else
            {
                ConsoleWriter.WriteText("[Food] ", ConsoleColor.Red, false);
                ConsoleWriter.WriteText("nothing ", null, false);
            }

            //Check attack consumption
            if (player.CurrentAttackId != null)
            {
                ConsoleWriter.WriteText($"[{player.CurrentAttackName}] ", ConsoleColor.Yellow, false);
                ConsoleWriter.WriteText($"{player.RemainingAttack}/{player.TotalAttack}  ", null, false);
            }
            else
            {
                ConsoleWriter.WriteText("[Attack] ", ConsoleColor.Red, false);
                ConsoleWriter.WriteText("nothing ", null, false);
            }

            //Check defense consumption
            if (player.CurrentDefenseId != null)
            {
                ConsoleWriter.WriteText($"[{player.CurrentDefenseName}] ", ConsoleColor.Yellow, false);
                ConsoleWriter.WriteText($"{player.RemainingDefense}/{player.TotalDefense}  ", null, false);
            }
            else
            {
                ConsoleWriter.WriteText("[Defense] ", ConsoleColor.Red, false);
                ConsoleWriter.WriteText("nothing ", null, false);
            }

            ConsoleWriter.WriteText("[Money] ", ConsoleColor.Yellow, false);
            ConsoleWriter.WriteText($"€{player.Money}  ", null, false);
            ConsoleWriter.WriteText("[Level] ", ConsoleColor.Yellow, false);
            ConsoleWriter.WriteText($"{player.GetLevel()} ({player.Experience}/{player.GetExperienceForNextLevel()})  ", null, false);

            ConsoleWriter.WriteText();
            ConsoleWriter.WriteText();
        }

        private async Task PerformAction(int playerId)
        {
            var result = await _gameApi.PerformActionAsync(playerId);

            var player = result.Data.Player;
            var positiveGameEvent = result.Data.PositiveGameEvent;
            var negativeGameEvent = result.Data.NegativeGameEvent;

            if (positiveGameEvent != null)
            {
                ConsoleWriter.WriteText($"{string.Format(_settings.ActionText, player.Name)} ",
                    ConsoleColor.Green, false);
                ConsoleWriter.WriteText(positiveGameEvent.Name, ConsoleColor.White);
                if (!string.IsNullOrWhiteSpace(positiveGameEvent.Description))
                {
                    ConsoleWriter.WriteText(positiveGameEvent.Description);
                }
                if (positiveGameEvent.Money > 0)
                {
                    ConsoleWriter.WriteText($"€{positiveGameEvent.Money}", ConsoleColor.Yellow, false);
                    ConsoleWriter.WriteText(" has been added to your account.");
                }
            }

            if (negativeGameEvent != null)
            {
                ConsoleWriter.WriteText(negativeGameEvent.Name, ConsoleColor.Blue);
                if (!string.IsNullOrWhiteSpace(negativeGameEvent.Description))
                {
                    ConsoleWriter.WriteText(negativeGameEvent.Description);
                }
                ConsoleWriter.WriteMessages(result.Data.NegativeGameEventMessages);
            }

            ConsoleWriter.WriteMessages(result.Messages);

            ConsoleWriter.WriteText();
        }

        private async Task Buy(int playerId, int itemId)
        {
            var result = await _gameApi.BuyAsync(playerId, itemId);

            if (result.IsSuccess)
            {
                ConsoleWriter.WriteText($"You bought {result.Data.Item.Name} for €{result.Data.Item.Price}");
                ConsoleWriter.WriteText($"Thank you for shopping. Your current balance is €{result.Data.Player.Money}.");

                //Check if there are info and warning messages
                var nonErrorMessages =
                    result.Messages.Where(m => m.MessagePriority == MessagePriority.Error).ToList();
                ConsoleWriter.WriteMessages(nonErrorMessages);
            }
            else
            {
                var errorMessages = result.Messages.Where(m => m.MessagePriority == MessagePriority.Error)
                    .ToList();
                ConsoleWriter.WriteMessages(errorMessages);
            }

            Console.WriteLine();
        }

        private int? GetIdParameterFromCommand(string command)
        {
            var commandParts = command.Split(" ");
            if (commandParts.Length == 1)
            {
                return null;
            }
            var idPart = commandParts[1];

            int.TryParse(idPart, out var itemId);

            return itemId;
        }
    }
}
