using System;
using System.Linq;
using System.Threading.Tasks;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Ui.ConsoleApp.Abstractions;
using ActionCommandGame.Ui.ConsoleApp.ConsoleWriters;
using ActionCommandGame.Ui.ConsoleApp.Navigation;
using ActionCommandGame.Ui.ConsoleApp.Stores;

namespace ActionCommandGame.Ui.ConsoleApp.Views
{
    internal class PlayerSelectionView: IView
    {
        private readonly MemoryStore _memoryStore;
        private readonly IPlayerApi _playerApi;
        private readonly NavigationManager _navigationManager;

        public PlayerSelectionView(
            MemoryStore memoryStore,
            IPlayerApi playerApi,
            NavigationManager navigationManager)
        {
            _memoryStore = memoryStore;
            _playerApi = playerApi;
            _navigationManager = navigationManager;
        }

        public async Task Show()
        {
            var playerId = await ReadPlayerId();

            if (!playerId.HasValue)
            {
                ConsoleBlockWriter.Write("Could not load players.", 1, ConsoleColor.Red);
                await _navigationManager.NavigateTo<ExitView>(false);
                return;
            }

            _memoryStore.CurrentPlayerId = playerId.Value;

           
            await _navigationManager.NavigateTo<GameView>();
        }

        private async Task<int?> ReadPlayerId()
        {
            var filter = new PlayerFilter { FilterUserPlayers = true };
            var players = await _playerApi.Find(filter);

            if (!players.Data.Any())
            {
                return null;
            }

            var playerTextLines = players.Data.Select(p => $"{p.Id}. {p.Name}").ToList();
            playerTextLines.Add("");
            playerTextLines.Add("Choose your Player by typing the correct number.");
            ConsoleBlockWriter.Write(playerTextLines, 1, ConsoleColor.White);
            ConsoleWriter.WriteText("Your player: ", ConsoleColor.White, false);
            var playerInput = Console.ReadLine();

            if (!int.TryParse(playerInput, out int playerId))
            {
                Console.Clear();

                ConsoleWriter.WriteText("Please enter a correct player number.", ConsoleColor.Blue);

                return await ReadPlayerId();
            }

            return playerId;
        }
    }
}
