using System;

namespace ActionCommandGame.Services.Helpers
{
    public class Dice
    {
        private readonly Random _random;

        public Dice()
        {
            _random = new Random();
        }

        public int RollDice(int diceSides)
        {
            return RollDice(1, diceSides);
        }

        public int RollDice(int rolls, int diceSides)
        {
            int result = 0;

            for (int i = 0; i < rolls; i++)
            {
                int nextRoll = _random.Next(0, diceSides + 1);
                if (nextRoll > result)
                    result = nextRoll;
            }

            return result;
        }

    }
}
