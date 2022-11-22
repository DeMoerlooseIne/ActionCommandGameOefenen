using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Extensions;
using ActionCommandGame.Services.Extensions.Filters;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Filters;
using ActionCommandGame.Services.Model.Results;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Services
{
    public class PlayerItemService : IPlayerItemService
    {
        private readonly ActionCommandGameDbContext _database;

        public PlayerItemService(ActionCommandGameDbContext database)
        {
            _database = database;
        }

        public async Task<ServiceResult<PlayerItemResult>> GetAsync(int id, string authenticatedUserId)
        {
            var playerItem = await _database.PlayerItems
                .ProjectToResult()
                .SingleOrDefaultAsync(pi => pi.Id == id);

            return new ServiceResult<PlayerItemResult>(playerItem);
        }

        public async Task<ServiceResult<IList<PlayerItemResult>>> FindAsync(PlayerItemFilter filter, string authenticatedUserId)
        {
            var playerItems = await _database.PlayerItems
                .ApplyFilter(filter)
                .ProjectToResult()
                .ToListAsync();

            return new ServiceResult<IList<PlayerItemResult>>(playerItems);
        }

        public async Task<ServiceResult<PlayerItemResult>> CreateAsync(int playerId, int itemId, string authenticatedUserId)
        {
            var player = _database.Players.SingleOrDefault(p => p.Id == playerId);
            if (player == null)
            {
                return new ServiceResult<PlayerItemResult>().PlayerNotFound();
            }

            var item = _database.Items.SingleOrDefault(i => i.Id == itemId);
            if (item == null)
            {
                return new ServiceResult<PlayerItemResult>().ItemNotFound();
            }

            var playerItem = new PlayerItem
            {
                ItemId = itemId,
                Item = item,
                PlayerId = playerId,
                Player = player
            };
            _database.PlayerItems.Add(playerItem);
            player.Inventory.Add(playerItem);
            item.PlayerItems.Add(playerItem);

            //Auto Equip the item you bought
            if (item.Fuel > 0)
            {
                playerItem.RemainingFuel = item.Fuel;
                player.CurrentFuelPlayerItemId = playerItem.Id;
                player.CurrentFuelPlayerItem = playerItem;
            }
            if (item.Attack > 0)
            {
                playerItem.RemainingAttack = item.Attack;
                player.CurrentAttackPlayerItemId = playerItem.Id;
                player.CurrentAttackPlayerItem = playerItem;
            }
            if (item.Defense > 0)
            {
                playerItem.RemainingDefense = item.Defense;
                player.CurrentDefensePlayerItemId = playerItem.Id;
                player.CurrentDefensePlayerItem = playerItem;
            }

            await _database.SaveChangesAsync();

            return await GetAsync(playerItem.Id, authenticatedUserId);
        }

        public async Task<ServiceResult> DeleteAsync(int id, string authenticatedUserId)
        {
            var playerItem = _database.PlayerItems.SingleOrDefault(pi => pi.Id == id);

            if (playerItem == null)
            {
                return new ServiceResult().NotFound();
            }
            
            var player = playerItem.Player;
            player.Inventory.Remove(playerItem);
            
            var item = playerItem.Item;
            item.PlayerItems.Remove(playerItem);

            //Clear up equipment
            if (player.CurrentFuelPlayerItemId == id)
            {
                player.CurrentFuelPlayerItemId = null;
                player.CurrentFuelPlayerItem = null;
            }
            if (player.CurrentAttackPlayerItemId == id)
            {
                player.CurrentAttackPlayerItemId = null;
                player.CurrentAttackPlayerItem = null;
            }
            if (player.CurrentDefensePlayerItemId == id)
            {
                player.CurrentDefensePlayerItemId = null;
                player.CurrentDefensePlayerItem = null;
            }

            _database.PlayerItems.Remove(playerItem);

            //Save Changes
            await _database.SaveChangesAsync();

            return new ServiceResult();
        }
        
    }
}
