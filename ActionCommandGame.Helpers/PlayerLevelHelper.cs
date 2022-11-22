namespace ActionCommandGame.Helpers
{
    public static class PlayerLevelHelper
    {
        public static int GetLevelFromExperience(int experience)
        {
            if (experience <= 0)
            {
                return 1;
            }
            return (int)Math.Floor((Math.Sqrt(100 * (2 * experience + 25)) + 50) / 100);
        }

        public static int GetExperienceFromLevel(int level)
        {
            if (level <= 0)
            {
                return 0;
            }

            return ((int)Math.Pow(level, 2) + level) / 2 * 100 - level * 100;
        }

        public static int GetExperienceForNextLevel(int experience)
        {
            var currentLevel = GetLevelFromExperience(experience);
            var nextLevel = currentLevel + 1;
            return GetExperienceFromLevel(nextLevel);
        }

        public static int GetRemainingExperienceUntilNextLevel(int experience)
        {
            var experienceForNextLevel = GetExperienceForNextLevel(experience);
            return experienceForNextLevel - experience;
        }
    }
}
