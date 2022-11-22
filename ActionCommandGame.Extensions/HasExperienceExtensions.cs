using ActionCommandGame.Abstractions;
using ActionCommandGame.Helpers;

namespace ActionCommandGame.Extensions
{
    public static class HasExperienceExtensions
    {
        public static int GetLevel(this IHasExperience player)
        {
            return PlayerLevelHelper.GetLevelFromExperience(player.Experience);
        }

        public static int GetExperienceForNextLevel(this IHasExperience player)
        {
            return PlayerLevelHelper.GetExperienceForNextLevel(player.Experience);
        }

        public static int GetLevelFromExperience(this IHasExperience player)
        {
            return PlayerLevelHelper.GetLevelFromExperience(player.Experience);
        }

        public static int GetRemainingExperienceUntilNextLevel(this IHasExperience player)
        {
            return PlayerLevelHelper.GetRemainingExperienceUntilNextLevel(player.Experience);
        }
    }
}
