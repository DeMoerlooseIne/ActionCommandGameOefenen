using ActionCommandGame.Model;
using ActionCommandGame.Repository.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ActionCommandGame.Repository
{
    public class ActionCommandGameDbContext: IdentityDbContext
    {
        public ActionCommandGameDbContext(DbContextOptions<ActionCommandGameDbContext> options): base(options)
        {
            
        }

        public DbSet<PositiveGameEvent> PositiveGameEvents { get; set; }
        public DbSet<NegativeGameEvent> NegativeGameEvents { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<PlayerItem> PlayerItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RemovePluralizingTableNameConvention();
            modelBuilder.ConfigureRelationships();

            base.OnModelCreating(modelBuilder);
        }

        public void Initialize()
        {
            var email = "bavo.ketels@vives.be";
            //Password Test123$
            var passwordHash = "AQAAAAEAACcQAAAAECp9VnV5jgDyqQqacxkrC+OcWFUM1+mavZ4+mxxhqtm/dg9UTVq1vhgAKFsblrEXDA==";
            var user = new IdentityUser
            {
                UserName = email,
                Email = email,
                NormalizedEmail = email.ToUpperInvariant(),
                NormalizedUserName = email.ToUpperInvariant(),
                PasswordHash = passwordHash
            };

            Users.Add(user);
            SaveChanges();

            GeneratePositiveGameEvents();
            GenerateNegativeGameEvents();
            GenerateAttackItems();
            GenerateDefenseItems();
            GenerateFoodItems();
            GenerateDecorativeItems();

            //God Mode Item
            Items.Add(new Item
            {
                Name = "GOD MODE",
                Description = "This is almost how a GOD must feel.",
                Attack = 1000000,
                Defense = 1000000,
                Fuel = 1000000,
                ActionCooldownSeconds = 1,
                Price = 10000000
            });

            Players.Add(new Player { UserId = user.Id, Name = "John Doe", Money = 100 });
            Players.Add(new Player { UserId = user.Id, Name = "John Francks", Money = 100000, Experience = 2000 });
            Players.Add(new Player { UserId = user.Id, Name = "Luc Doleman", Money = 500, Experience = 5 });
            Players.Add(new Player { UserId = user.Id, Name = "Emilio Fratilleci", Money = 12345, Experience = 200 });

            SaveChanges();
        }

        private void GeneratePositiveGameEvents()
        {
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Nothing but boring rocks", Probability = 1000 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "The biggest Opal you ever saw.", Description = "It slips out of your hands and rolls inside a crack in the floor. It is out of reach.", Probability = 500 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Sand, dirt and dust", Probability = 1000 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "A piece of empty paper", Description = "You hold it to the light and warm it up to reveal secret texts, but it remains empty.", Probability = 1000 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "A small water stream", Description = "The water flows around your feet and creates a dirty puddle.", Probability = 1000 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Junk", Money = 1, Experience = 1, Probability = 2000 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Murphy's idea bin", Money = 1, Experience = 1, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Donald's book of excuses", Money = 1, Experience = 1, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Children's Treasure Map", Money = 1, Experience = 1, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Trinket", Money = 5, Experience = 3, Probability = 1000 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Old Tool", Money = 10, Experience = 5, Probability = 800 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Old Equipment", Money = 10, Experience = 5, Probability = 800 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Ornate Shell", Money = 10, Experience = 5, Probability = 800 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Fossil", Money = 12, Experience = 6, Probability = 700 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Cave Shroom", Money = 20, Experience = 8, Probability = 650 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Artifact", Money = 30, Experience = 10, Probability = 500 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Scrap Metal", Money = 50, Experience = 13, Probability = 400 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Jewelry", Money = 60, Experience = 15, Probability = 400 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Peculiar Mask", Money = 100, Experience = 40, Probability = 350 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Quartz Geode", Money = 140, Experience = 50, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Ancient Weapon", Money = 160, Experience = 80, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Ancient Instrument", Money = 160, Experience = 80, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Ancient Texts", Money = 180, Experience = 80, Probability = 300 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Gemstone", Money = 300, Experience = 100, Probability = 110 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Mysterious Potion", Money = 300, Experience = 100, Probability = 80 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Meteorite", Money = 400, Experience = 150, Probability = 200 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Ancient Bust", Money = 500, Experience = 150, Probability = 150 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Buried Treasure", Money = 1000, Experience = 200, Probability = 100 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Alien DNA", Money = 60000, Experience = 1500, Probability = 5 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Rare Collector's Item", Money = 3000, Experience = 400, Probability = 30 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Pure Gold", Money = 2000, Experience = 350, Probability = 30 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Safe Deposit Box Key", Money = 20000, Experience = 1000, Probability = 10 });
            PositiveGameEvents.Add(new PositiveGameEvent { Name = "Advanced Bio Tech", Money = 30000, Experience = 1500, Probability = 10 });
        }

        public void GenerateNegativeGameEvents()
        {
            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Rockfall",
                Description = "As you are mining, the cave walls rumble and rocks tumble down on you",
                DefenseWithGearDescription = "Your mining gear allows you and your tools to escape unscathed",
                DefenseWithoutGearDescription = "You try to cover your face but the rocks are too heavy. That hurt!",
                DefenseLoss = 2,
                Probability = 100
            });
            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Cave Rat",
                Description = "As you are mining, you feel something scurry between your feet!",
                DefenseWithGearDescription = "It tries to bite you, but your mining gear keeps the rat's teeth from sinking in.",
                DefenseWithoutGearDescription = "It tries to bite you and nicks you in the ankles. It already starts to glow dangerously.",
                DefenseLoss = 3,
                Probability = 50
            });
            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Sinkhole",
                Description = "As you are mining, the ground suddenly gives way and you fall down into a chasm!",
                DefenseWithGearDescription = "Your gear grants a safe landing, protecting you and your pickaxe.",
                DefenseWithoutGearDescription = "You tumble down the dark hole and take a really bad landing. That hurt!",
                DefenseLoss = 2,
                Probability = 100
            });
            NegativeGameEvents.Add(new NegativeGameEvent
            {
                Name = "Ancient Bacteria",
                Description = "As you are mining, you uncover a green slime oozing from the cracks!",
                DefenseWithGearDescription = "Your gear barely covers you from the noxious goop. You are safe.",
                DefenseWithoutGearDescription = "The slime covers your hands and arms and starts biting through your flesh. This hurts!",
                DefenseLoss = 3,
                Probability = 50
            });
        }

        private void GenerateAttackItems()
        {
            Items.Add(new Item { Name = "Basic Pickaxe", Attack = 50, Price = 50 });
            Items.Add(new Item { Name = "Enhanced Pick", Attack = 300, Price = 300 });
            Items.Add(new Item { Name = "Turbo Pick", Attack = 500, Price = 500 });
            Items.Add(new Item { Name = "Mithril Warpick", Attack = 5000, Price = 15000 });
            Items.Add(new Item { Name = "Thor's Hammer", Attack = 50, Price = 1000000 });
        }

        private void GenerateDefenseItems()
        {
            Items.Add(new Item { Name = "Torn Clothes", Defense = 20, Price = 20 });
            Items.Add(new Item { Name = "Hardened Leather Gear", Defense = 150, Price = 200 });
            Items.Add(new Item { Name = "Iron plated Armor", Defense = 500, Price = 1000 });
            Items.Add(new Item { Name = "Rock Shield", Defense = 2000, Price = 10000 });
            Items.Add(new Item { Name = "Emerald Shield", Defense = 2000, Price = 10000 });
            Items.Add(new Item { Name = "Diamond Shield", Defense = 20000, Price = 10000 });
        }

        private void GenerateFoodItems()
        {
            Items.Add(new Item { Name = "Apple", ActionCooldownSeconds = 50, Fuel = 4, Price = 8 });
            Items.Add(new Item { Name = "Energy Bar", ActionCooldownSeconds = 45, Fuel = 5, Price = 10 });
            Items.Add(new Item { Name = "Field Rations", ActionCooldownSeconds = 30, Fuel = 30, Price = 300 });
            Items.Add(new Item { Name = "Abbye cheese", ActionCooldownSeconds = 25, Fuel = 100, Price = 500 });
            Items.Add(new Item { Name = "Abbye Beer", ActionCooldownSeconds = 25, Fuel = 100, Price = 500 });
            Items.Add(new Item { Name = "Celestial Burrito", ActionCooldownSeconds = 15, Fuel = 500, Price = 10000 });
#if DEBUG
            Items.Add(new Item { Name = "Developer Food", ActionCooldownSeconds = 1, Fuel = 1000, Price = 1 });
#endif
        }

        private void GenerateDecorativeItems()
        {
            Items.Add(new Item { Name = "Balloon", Description = "Does nothing. Do you feel special now?", Price = 10 });
            Items.Add(new Item { Name = "Blue Medal", Description = "For those who cannot afford the Crown of Flexing.", Price = 100000 });
            Items.Add(new Item { Name = "Crown of Flexing", Description = "Yes, show everyone how much money you are willing to spend on something useless!", Price = 500000 });
        }

    }
}
