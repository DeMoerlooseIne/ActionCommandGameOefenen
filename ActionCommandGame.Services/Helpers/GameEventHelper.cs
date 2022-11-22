using System.Collections.Generic;
using System.Linq;
using ActionCommandGame.Abstractions;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Services.Helpers
{
    /// <summary>
    /// https://stackoverflow.com/a/26002723/878132
    /// Used answer from John B. Lambe to inspire me
    /// </summary>
    public static class GameEventHelper
    {
        public static PositiveGameEventResult GetRandomPositiveGameEvent(IList<PositiveGameEventResult> gameEvents)
        {
            //Calculate the chance based on the total probability of all gameEvents
            var dice = new Dice();
            var totalProbability = gameEvents.Sum(ge => ge.Probability);
            var diceRoll = dice.RollDice(totalProbability);

            var cumulativeGameEvents = CreateCumulativeGameEvents(gameEvents);
            //Get the first one that is larger than the DiceRoll
            var cumulativeGameEvent = cumulativeGameEvents.FirstOrDefault(cge => cge.CumulativeProbability >= diceRoll);

            return cumulativeGameEvent?.GameEvent;
        }

        public static NegativeGameEventResult GetRandomNegativeGameEvent(IList<NegativeGameEventResult> gameEvents)
        {
            var dice = new Dice();
            //~10% chance on a negative game event
            if (dice.RollDice(10) > 1)
            {
                return null;
            }

            //Calculate the chance based on the total probability of all gameEvents
            var totalProbability = gameEvents.Sum(ge => ge.Probability);
            var diceRoll = dice.RollDice(totalProbability);

            var cumulativeGameEvents = CreateCumulativeGameEvents(gameEvents);
            //Get the first one that is larger than the DiceRoll
            var cumulativeGameEvent = cumulativeGameEvents.FirstOrDefault(cge => cge.CumulativeProbability > diceRoll);

            return cumulativeGameEvent?.GameEvent;
        }

        //Generate a cumulative list, where each probability is added to the previous
        private static IEnumerable<CumulativeGameEvent<T>> CreateCumulativeGameEvents<T>(IList<T> gameEvents)
            where T: IHasProbability
        {
            int currentCumulativeProbability = 0;
            foreach (var gameEvent in gameEvents)
            {
                currentCumulativeProbability += gameEvent.Probability;
                yield return new CumulativeGameEvent<T> { GameEvent = gameEvent, CumulativeProbability = currentCumulativeProbability };
            }

            /*
             * Example:
             * ========
             *
             * Type                 Probability  Cumulative
             * ----                 -----------  ----------
             * Bloodstone             10            10              (0..9 yield Bloodstone)
             * Copper                 67            77    (10+67)   (10..76 yield Copper)
             * Emeraldite             29           105    (77+29)
             * Gold                   20           125    etc.
             * Heronite               17           142
             * Platinum               17           159
             * Shadownite             13           172
             * Silver                 29           200
             * Soranite                1           201
             * Umbrarite               1           202
             * Cobalt                 13           216
             * Iron                   67           282
             *
             */
        }
    }
}
