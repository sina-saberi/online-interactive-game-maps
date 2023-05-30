using game_maps.Application.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_maps.Application.ViewModels
{
    public class LocationDetailViewModel : BaseViewModel
    {
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string? Features { get; set; }
        public string? IgnMarkerId { get; set; }
        public string? IgnPageId { get; set; }
        public UserLocationMarkViewModel? Marker { get; set; }
        public ICollection<MediaViewModel>? Medias { get; set; }
    }
}
