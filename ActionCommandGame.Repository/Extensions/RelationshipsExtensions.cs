using ActionCommandGame.Model;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Repository.Extensions
{
    public static class RelationshipsExtensions
    {
        public static void ConfigureRelationships(this ModelBuilder builder)
        {
            builder.ConfigurePlayerItem();
            builder.ConfigurePlayer();
        }

        private static void ConfigurePlayerItem(this ModelBuilder builder)
        {
            builder.Entity<PlayerItem>()
                .HasOne(a => a.Item)
                .WithMany(u => u.PlayerItems)
                .HasForeignKey(a => a.ItemId);

            builder.Entity<PlayerItem>()
                .HasOne(a => a.Player)
                .WithMany(u => u.Inventory)
                .HasForeignKey(a => a.PlayerId);
        }

        private static void ConfigurePlayer(this ModelBuilder builder)
        {
            builder.Entity<Player>()
                .HasOne(a => a.CurrentFuelPlayerItem)
                .WithMany(u => u.FuelPlayers)
                .HasForeignKey(a => a.CurrentFuelPlayerItemId);

            builder.Entity<Player>()
                .HasOne(a => a.CurrentAttackPlayerItem)
                .WithMany(u => u.AttackPlayers)
                .HasForeignKey(a => a.CurrentAttackPlayerItemId);

            builder.Entity<Player>()
                .HasOne(a => a.CurrentDefensePlayerItem)
                .WithMany(u => u.DefensePlayers)
                .HasForeignKey(a => a.CurrentDefensePlayerItemId);
        }
    }
}
