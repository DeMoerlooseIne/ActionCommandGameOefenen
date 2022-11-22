using ActionCommandGame.Abstractions;

namespace ActionCommandGame.Model
{
    public class PositiveGameEvent: IIdentifiable, IHasProbability
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Money { get; set; }
        public int Experience { get; set; }
        public int Probability { get; set; }
    }
}
