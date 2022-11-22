using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActionCommandGame.Extensions;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Ui.ConsoleApp.Abstractions;
using ActionCommandGame.Ui.ConsoleApp.ConsoleWriters;
using ActionCommandGame.Ui.ConsoleApp.Stores;

namespace ActionCommandGame.Ui.ConsoleApp.Views
{
    internal class LeaderboardView: IView
    {
        private readonly MemoryStore _memoryStore;
        private readonly IPlayerApi _playerApi;

        public LeaderboardView(
            MemoryStore memoryStore,
            IPlayerApi playerApi)
        {
            _memoryStore = memoryStore;
            _playerApi = playerApi;
        }

        public async Task Show()
        {
            ConsoleBlockWriter.Write("Leaderboard");

            var playersResult = await _playerApi.Find(new PlayerFilter());
            var orderedPlayers = playersResult.Data.OrderByDescending(p => p.Experience).ToList();

            foreach (var player in orderedPlayers)
            {
                var text = $"\tLevel {player.GetLevel()} {player.Name} ({player.Experience})";
                if (player.Id == _memoryStore.CurrentPlayerId)
                {
                    ConsoleWriter.WriteText(text, ConsoleColor.Yellow);
                }
                else
                {
                    ConsoleWriter.WriteText(text);
                }
            }
        }
    }
}
