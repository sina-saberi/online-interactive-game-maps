using game_maps.Application.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_maps.Application.ViewModels
{
    public class UserLocationMarkViewModel : BaseViewModel
    {
        public bool IsDone { get; set; }
        public string Note { get; set; } = "";
        public int? Id { get; set; }
        public int LocationId { get; set; }
    }
}
