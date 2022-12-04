using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Ui.WebApp.Models
{
    public class PlayerShop
    {
        public PlayerResult Player { get; set; }
        public int Id { get; set; }
        public IList<ItemResult> Items { get; set; }
    }
}
