namespace ActionCommandGame.Services.Model.Results
{
    public class ItemResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int Price { get; set; }
        public int Fuel { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int ActionCooldownSeconds { get; set; }
    }
}
