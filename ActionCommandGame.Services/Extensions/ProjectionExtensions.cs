using System.Linq;
using ActionCommandGame.Model;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Extensions
{
    public static class ProjectionExtensions
    {
        public static IQueryable<PlayerResult> ProjectToResult(this IQueryable<Player> query)
        {
            return query.Select(p => new PlayerResult
            {
                Id = p.Id,
                Name = p.Name,
                UserId = p.UserId,
                Experience = p.Experience,
                Money = p.Money,
                LastActionExecutedDateTime = p.LastActionExecutedDateTime,
                CurrentAttackId = p.CurrentAttackPlayerItemId,
                CurrentAttackName = p.CurrentAttackPlayerItemId.HasValue ? p.CurrentAttackPlayerItem.Item.Name : string.Empty,
                CurrentDefenseId = p.CurrentDefensePlayerItemId,
                CurrentDefenseName = p.CurrentDefensePlayerItemId.HasValue ? p.CurrentDefensePlayerItem.Item.Name : string.Empty,
                CurrentFuelId = p.CurrentFuelPlayerItemId,
                CurrentFuelName = p.CurrentFuelPlayerItemId.HasValue ? p.CurrentFuelPlayerItem.Item.Name : string.Empty,
                CurrentFuelActionCooldownSeconds = p.CurrentFuelPlayerItemId.HasValue ? p.CurrentFuelPlayerItem.Item.ActionCooldownSeconds : 0,
                RemainingFuel = p.CurrentFuelPlayerItemId.HasValue ? p.CurrentFuelPlayerItem.RemainingFuel : 0,
                TotalFuel = p.CurrentFuelPlayerItemId.HasValue ? p.CurrentFuelPlayerItem.Item.Fuel : 0,
                RemainingAttack = p.CurrentAttackPlayerItemId.HasValue ? p.CurrentAttackPlayerItem.RemainingAttack : 0,
                TotalAttack = p.CurrentAttackPlayerItemId.HasValue ? p.CurrentAttackPlayerItem.Item.Attack : 0,
                RemainingDefense = p.CurrentDefensePlayerItemId.HasValue ? p.CurrentDefensePlayerItem.RemainingDefense : 0,
                TotalDefense = p.CurrentDefensePlayerItemId.HasValue ? p.CurrentDefensePlayerItem.Item.Defense : 0,
                NumberOfInventoryItems = p.Inventory.Count
            });
        }

        public static IQueryable<PositiveGameEventResult> ProjectToResult(this IQueryable<PositiveGameEvent> query)
        {
            return query.Select(pge => new PositiveGameEventResult
            {
                Id = pge.Id,
                Name = pge.Name,
                Description = pge.Description,
                Experience = pge.Experience,
                Money = pge.Money,
                Probability = pge.Probability
            });
        }

        public static IQueryable<NegativeGameEventResult> ProjectToResult(this IQueryable<NegativeGameEvent> query)
        {
            return query.Select(nge => new NegativeGameEventResult
            {
                Id=nge.Id,
                Name = nge.Name,
                Description = nge.Description,
                DefenseLoss = nge.DefenseLoss,
                DefenseWithGearDescription = nge.DefenseWithGearDescription,
                DefenseWithoutGearDescription = nge.DefenseWithoutGearDescription,
                Probability = nge.Probability
            });
        }

        public static IQueryable<PlayerItemResult> ProjectToResult(this IQueryable<PlayerItem> query)
        {
            return query.Select(pi => new PlayerItemResult
            {
                Id = pi.Id,
                ItemId = pi.ItemId,
                Name = pi.Item.Name,
                Description = pi.Item.Description,
                Fuel = pi.Item.Fuel,
                Attack = pi.Item.Attack,
                Defense = pi.Item.Defense,
                ActionCooldownSeconds = pi.Item.ActionCooldownSeconds,
                PlayerId = pi.PlayerId,
                PlayerName = pi.Player.Name,
                RemainingAttack = pi.RemainingAttack,
                RemainingDefense = pi.RemainingDefense,
                RemainingFuel = pi.RemainingFuel
            });
        }

        public static IQueryable<ItemResult> ProjectToResult(this IQueryable<Item> query)
        {
            return query.Select(i => new ItemResult
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                Price = i.Price,
                ActionCooldownSeconds = i.ActionCooldownSeconds,
                Attack = i.Attack,
                Defense = i.Defense,
                Fuel = i.Fuel
            });
        }
    }
}
