using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ActionCommandGame.Extensions;
using ActionCommandGame.Model;
using ActionCommandGame.Repository;
using ActionCommandGame.Services.Abstractions;
using ActionCommandGame.Services.Extensions;
using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Results;
using ActionCommandGame.Services.Settings;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Services
{
    public class GameService : IGameService
    {
        private readonly GameSettings _gameSettings;
        private readonly ActionCommandGameDbContext _database;
        private readonly IPlayerService _playerService;
        private readonly IPositiveGameEventService _positiveGameEventService;
        private readonly INegativeGameEventService _negativeGameEventService;
        private readonly IItemService _itemService;
        private readonly IPlayerItemService _playerItemService;

        public GameService(
            GameSettings gameSettings,
            ActionCommandGameDbContext database,
            IPlayerService playerService,
            IPositiveGameEventService positiveGameEventService,
            INegativeGameEventService negativeGameEventService,
            IItemService itemService,
            IPlayerItemService playerItemService)
        {
            _gameSettings = gameSettings;
            _database = database;
            _playerService = playerService;
            _positiveGameEventService = positiveGameEventService;
            _negativeGameEventService = negativeGameEventService;
            _itemService = itemService;
            _playerItemService = playerItemService;
        }

        public async Task<ServiceResult<GameResult>> PerformActionAsync(int playerId, string authenticatedUserId)
        {
            //Check Cooldown
            var player = await _database.Players
                .Include(p => p.CurrentFuelPlayerItem.Item)
                .SingleOrDefaultAsync(p => p.Id == playerId);

            ServiceResult<PlayerResult> playerResult;

            if (player is null)
            {
                return new ServiceResult<GameResult>
                {
                    Messages = new List<ServiceMessage>
                        { new ServiceMessage { Code = "NotFound", Message = $"{nameof(Player)} was not found" } }
                };
            }

            if (player.LastActionExecutedDateTime.HasValue)
            {
	            var elapsedSeconds = DateTime.UtcNow.Subtract(player.LastActionExecutedDateTime.Value).TotalSeconds;
	            var cooldownSeconds = _gameSettings.DefaultCooldown;

                if (player.CurrentFuelPlayerItemId.HasValue)
                {
                    cooldownSeconds = player.CurrentFuelPlayerItem.Item.ActionCooldownSeconds;
                }

                if (elapsedSeconds < cooldownSeconds)
                {
                    //Save Player
                    await _database.SaveChangesAsync();

                    var waitSeconds = Math.Ceiling(cooldownSeconds - elapsedSeconds);
                    var waitText = $"You are still a bit tired. You have to wait another {waitSeconds} seconds.";
                    playerResult = await _playerService.GetAsync(playerId, authenticatedUserId);
                    return new ServiceResult<GameResult>
                    {
                        Data = new GameResult { Player = playerResult.Data },
                        Messages = new List<ServiceMessage> { new ServiceMessage { Code = "Cooldown", Message = waitText } }
                    };
                }
            }

            player.LastActionExecutedDateTime = DateTime.UtcNow;
            
            var hasAttackItem = player.CurrentAttackPlayerItemId.HasValue;
            var positiveGameEvent = await _positiveGameEventService.GetRandomPositiveGameEvent(hasAttackItem, authenticatedUserId);
            if (positiveGameEvent.Data is null)
            {
                return new ServiceResult<GameResult>{Messages = 
                    new List<ServiceMessage>
                    {
                        new ServiceMessage
                        {
                            Code = "Error",
                            Message = "Something went wrong getting the Positive Game Event.",
                            MessagePriority = MessagePriority.Error
                        }
                    }};
            }

            var negativeGameEvent = await _negativeGameEventService.GetRandomNegativeGameEvent(authenticatedUserId);

            var oldLevel = player.GetLevel();

            player.Money += positiveGameEvent.Data.Money;
            player.Experience += positiveGameEvent.Data.Experience;

            var newLevel = player.GetLevel();

            var levelMessages = new List<ServiceMessage>();
            //Check if we leveled up
            if (oldLevel < newLevel)
            {
                levelMessages = new List<ServiceMessage>{new ServiceMessage{Code="LevelUp", Message = $"Congratulations, you arrived at level {newLevel}"}};
            }

            //Consume fuel
            var fuelMessages = ConsumeFuel(player, authenticatedUserId);

            var attackMessages = new List<ServiceMessage>();
            //Consume attack when we got some loot
            if (positiveGameEvent.Data.Money > 0)
            {
                attackMessages.AddRange(ConsumeAttack(player, authenticatedUserId));
            }

            var defenseMessages = new List<ServiceMessage>();
            var negativeGameEventMessages = new List<ServiceMessage>();
            if (negativeGameEvent.Data is not null)
            {
                //Check defense consumption
                if (player.CurrentDefensePlayerItem != null)
                {
                    negativeGameEventMessages.Add(new ServiceMessage { Code = "DefenseWithGear", Message = negativeGameEvent.Data.DefenseWithGearDescription });
                    defenseMessages.AddRange(ConsumeDefense(player, authenticatedUserId, negativeGameEvent.Data.DefenseLoss));
                }
                else
                {
                    negativeGameEventMessages.Add(new ServiceMessage { Code = "DefenseWithoutGear", Message = negativeGameEvent.Data.DefenseWithoutGearDescription });

                    //If we have no defense item, consume the defense loss from Fuel and Attack
                    defenseMessages.AddRange(ConsumeFuel(player, authenticatedUserId, negativeGameEvent.Data.DefenseLoss));
                    defenseMessages.AddRange(ConsumeAttack(player, authenticatedUserId, negativeGameEvent.Data.DefenseLoss));
                }
            }

            var warningMessages = GetWarningMessages(player);

            //Save Player
            await _database.SaveChangesAsync();

            playerResult = await _playerService.GetAsync(playerId, authenticatedUserId);
            var gameResult = new GameResult
            {
                Player = playerResult.Data,
                PositiveGameEvent = positiveGameEvent.Data,
                NegativeGameEvent = negativeGameEvent.Data,
                NegativeGameEventMessages = negativeGameEventMessages
            };

            var serviceResult = new ServiceResult<GameResult>
            {
                Data = gameResult
            };

            //Add all the messages to the player
            serviceResult.WithMessages(levelMessages);
            serviceResult.WithMessages(warningMessages);
            serviceResult.WithMessages(fuelMessages);
            serviceResult.WithMessages(attackMessages);
            serviceResult.WithMessages(defenseMessages);

            return serviceResult;
        }

        public async Task<ServiceResult<BuyResult>> BuyAsync(int playerId, int itemId, string authenticatedUserId)
        {
            var player = await _database.Players.SingleOrDefaultAsync(p => p.Id == playerId);
            if (player == null)
            {
                return new ServiceResult<BuyResult>().PlayerNotFound();
            }

            var item = await _database.Items.SingleOrDefaultAsync(i => i.Id == itemId);
            if (item == null)
            {
                return new ServiceResult<BuyResult>().ItemNotFound();
            }

            if (item.Price > player.Money)
            {
                return new ServiceResult<BuyResult>().NotEnoughMoney();
            }

            await _playerItemService.CreateAsync(playerId, itemId, authenticatedUserId);

            player.Money -= item.Price;

            //SaveChanges
            await _database.SaveChangesAsync();

            //Get the result objects
            var playerResult = await _playerService.GetAsync(playerId, authenticatedUserId);
            var itemResult = await _itemService.GetAsync(itemId, authenticatedUserId);

            var buyResult = new BuyResult
            {
                Player = playerResult.Data,
                Item = itemResult.Data
            };
            return new ServiceResult<BuyResult> { Data = buyResult };
        }

        private IList<ServiceMessage> ConsumeFuel(Player player, string authenticatedUserId, int fuelLoss = 1)
        {
            if (player.CurrentFuelPlayerItem != null && player.CurrentFuelPlayerItemId.HasValue)
            {
                player.CurrentFuelPlayerItem.RemainingFuel -= fuelLoss;
                if (player.CurrentFuelPlayerItem.RemainingFuel <= 0)
                {
                    _playerItemService.DeleteAsync(player.CurrentFuelPlayerItemId.Value, authenticatedUserId);

                    //Load a new Fuel Item from inventory
                    var newFuelItem = _database.PlayerItems
                        .Where(pi => pi.PlayerId == player.Id && pi.Item.Fuel > 0)
                        .OrderByDescending(pi => pi.Item.Fuel).FirstOrDefault();

                    if (newFuelItem != null)
                    {
                        player.CurrentFuelPlayerItem = newFuelItem;
                        player.CurrentFuelPlayerItemId = newFuelItem.Id;
                        return new List<ServiceMessage>{new ServiceMessage
                        {
                            Code = "ReloadedFuel",
                            Message = $"You are hungry and open a new {newFuelItem.Item.Name}. Yummy!"
                        }};
                    }

                    return new List<ServiceMessage>{new ServiceMessage
                    {
                        Code = "NoFood",
                        Message = "You are so hungry. You look into your bag and find ... nothing!",
                        MessagePriority = MessagePriority.Warning
                    }};
                }
            }

            return new List<ServiceMessage>();
        }

        private IList<ServiceMessage> ConsumeAttack(Player player, string authenticatedUserId, int attackLoss = 1)
        {
            if (player.CurrentAttackPlayerItem != null && player.CurrentAttackPlayerItemId.HasValue)
            {
                var oldAttackItemName = player.CurrentAttackPlayerItem.Item.Name;
                player.CurrentAttackPlayerItem.RemainingAttack -= attackLoss;
                if (player.CurrentAttackPlayerItem.RemainingAttack <= 0)
                {
                    _playerItemService.DeleteAsync(player.CurrentAttackPlayerItemId.Value, authenticatedUserId);

                    //Load a new Attack Item from inventory
                    var newAttackItem = _database.PlayerItems
                        .Where(pi => pi.PlayerId == player.Id && pi.Item.Attack > 0)
                        .OrderByDescending(pi => pi.Item.Attack).FirstOrDefault();
                    if (newAttackItem != null)
                    {
                        player.CurrentAttackPlayerItem = newAttackItem;
                        player.CurrentAttackPlayerItemId = newAttackItem.Id;
                        return new List<ServiceMessage>{new ServiceMessage
                        {
                            Code = "ReloadedAttack",
                            Message = $"You just broke {oldAttackItemName}. No worries, you swiftly wield a new {newAttackItem.Item.Name}. Yeah!",

                        }};
                    }

                    return new List<ServiceMessage>{new ServiceMessage
                    {
                        Code = "NoAttack",
                        Message = $"You just broke {oldAttackItemName}. This was your last tool. Bummer!",
                        MessagePriority = MessagePriority.Warning
                    }};
                }
            }
            else
            {
                //If we don't have any attack tools, just consume more fuel in stead
                ConsumeFuel(player, authenticatedUserId);
            }

            return new List<ServiceMessage>();
        }

        private IList<ServiceMessage> ConsumeDefense(Player player, string authenticatedUserId, int defenseLoss = 1)
        {
            if (player.CurrentDefensePlayerItem != null && player.CurrentDefensePlayerItemId.HasValue)
            {
                var oldDefenseItemName = player.CurrentDefensePlayerItem.Item.Name;
                player.CurrentDefensePlayerItem.RemainingDefense -= defenseLoss;
                if (player.CurrentDefensePlayerItem.RemainingDefense <= 0)
                {
                    _playerItemService.DeleteAsync(player.CurrentDefensePlayerItemId.Value, authenticatedUserId);

                    //Load a new Defense Item from inventory
                    var newDefenseItem = _database.PlayerItems
                        .Where(pi => pi.PlayerId == player.Id && pi.Item.Defense > 0)
                        .OrderByDescending(pi => pi.Item.Defense).FirstOrDefault();
                    
                    if (newDefenseItem != null)
                    {
                        player.CurrentDefensePlayerItem = newDefenseItem;
                        player.CurrentDefensePlayerItemId = newDefenseItem.Id;

                        return new List<ServiceMessage>{new ServiceMessage
                        {
                            Code = "ReloadedDefense",
                            Message = $"Your {oldDefenseItemName} is starting to smell. No worries, you swiftly put on a freshly washed {newDefenseItem.Item.Name}. Yeah!"
                        }};
                    }

                    return new List<ServiceMessage>{new ServiceMessage
                    {
                        Code = "NoAttack",
                        Message = $"You just lost {oldDefenseItemName}. You continue without protection. Did I just see something move?",
                        MessagePriority = MessagePriority.Warning
                    }};
                }
            }
            else
            {
                //If we don't have defensive gear, just consume more fuel in stead.
                ConsumeFuel(player, authenticatedUserId);
            }

            return new List<ServiceMessage>();
        }

        private IList<ServiceMessage> GetWarningMessages(Player player)
        {
            var serviceMessages = new List<ServiceMessage>();

            if (player.CurrentFuelPlayerItem == null)
            {
                var infoText = "Playing without food is hard. You need a long time to recover. Consider buying food from the shop.";
                serviceMessages.Add(new ServiceMessage { Code = "NoFood", Message = infoText, MessagePriority = MessagePriority.Warning });
            }
            if (player.CurrentAttackPlayerItem == null)
            {
                var infoText = "Playing without tools is hard. You lost extra fuel. Consider buying tools from the shop.";
                serviceMessages.Add(new ServiceMessage { Code = "NoTools", Message = infoText, MessagePriority = MessagePriority.Warning });
            }
            if (player.CurrentDefensePlayerItem == null)
            {
                var infoText = "Playing without gear is hard. You lost extra fuel. Consider buying gear from the shop.";
                serviceMessages.Add(new ServiceMessage { Code = "NoGear", Message = infoText, MessagePriority = MessagePriority.Warning });
            }

            return serviceMessages;
        }
    }
}
