using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ActionCommandGame.Services.Model.Results;

namespace ActionCommandGame.Ui.WebApp.Models
{
    public class User
    {

        [Required]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        public IList<PlayerResult> Players { set; get; }

    }
}
