using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActionCommandGame.Sdk.Abstractions;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Services.Model.Results;
using ActionCommandGame.Ui.ConsoleApp.Abstractions;
using ActionCommandGame.Ui.ConsoleApp.ConsoleWriters;
using ActionCommandGame.Ui.ConsoleApp.Stores;

namespace ActionCommandGame.Ui.ConsoleApp.Views
{
    internal class InventoryView: IView
    {
        private readonly MemoryStore _memoryStore;
        private readonly IPlayerItemApi _playerItemApi;

        public InventoryView(
            MemoryStore memoryStore,
            IPlayerItemApi playerItemApi)
        {
            _memoryStore = memoryStore;
            _playerItemApi = playerItemApi;
        }
        public async Task Show()
        {
            ConsoleBlockWriter.Write("Inventory");

            var filter = new PlayerItemFilter { PlayerId = _memoryStore.CurrentPlayerId };
            var inventoryResult = await _playerItemApi.FindAsync(filter);

            foreach (var playerItem in inventoryResult.Data)
            {
                ShowPlayerItem(playerItem);
            }
        }

        private static void ShowPlayerItem(PlayerItemResult playerItem)
        {
            ConsoleWriter.WriteText($"\t{playerItem.Name}", ConsoleColor.White);
            if (!string.IsNullOrWhiteSpace(playerItem.Description))
            {
                ConsoleWriter.WriteText($"\t\t{playerItem.Description}");
            }
            if (playerItem.Fuel > 0)
            {
                ConsoleWriter.WriteText($"\t\tFuel: {playerItem.RemainingFuel}/{playerItem.Fuel}");
            }
            if (playerItem.Attack > 0)
            {
                ConsoleWriter.WriteText($"\t\tAttack: {playerItem.RemainingAttack}/{playerItem.Attack}");
            }
            if (playerItem.Defense > 0)
            {
                ConsoleWriter.WriteText($"\t\tDefense: {playerItem.RemainingDefense}/{playerItem.Defense}");
            }
            if (playerItem.ActionCooldownSeconds > 0)
            {
                ConsoleWriter.WriteText($"\t\tCooldown seconds: {playerItem.ActionCooldownSeconds}");
            }
        }
    }
}
