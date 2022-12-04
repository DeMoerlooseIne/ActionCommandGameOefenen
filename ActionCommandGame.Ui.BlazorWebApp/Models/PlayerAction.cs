using ActionCommandGame.Services.Model.Core;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Ui.WebApp.Models
{
    public class PlayerAction
    {
        public PlayerResult Player { get; set; }
        public int Id { get; set; }
        public GameResult? GameResult { get; set; }
        public IList<ServiceMessage>? Messages { get; set; }
        public IList<PlayerItemResult>? Items { get; set; }
    }
}
