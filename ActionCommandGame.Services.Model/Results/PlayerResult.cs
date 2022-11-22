using System;
using ActionCommandGame.Abstractions;

namespace ActionCommandGame.Services.Model.Results
{
    public class PlayerResult: IHasExperience
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Money { get; set; }
        public int Experience { get; set; }
        public DateTime? LastActionExecutedDateTime { get; set; }
        public string UserId { get; set; }
        public int? CurrentFuelId { get; set; }
        public string CurrentFuelName { get; set; }
        public int CurrentFuelActionCooldownSeconds { get; set; }
        public int TotalFuel { get; set; }
        public int RemainingFuel { get; set; }
        public int? CurrentAttackId { get; set; }
        public string CurrentAttackName { get; set; }
        public int TotalAttack { get; set; }
        public int RemainingAttack { get; set; }
        public int? CurrentDefenseId { get; set; }
        public string CurrentDefenseName { get; set; }
        public int TotalDefense { get; set; }
        public int RemainingDefense { get; set; }
        public int NumberOfInventoryItems { get; set; }
    }
}
