using AutoMapper;
using game_maps.Application.Base;
using game_maps.Application.ViewModels;
using game_maps.Core.Base;
using game_maps.Core.Entities;

namespace game_maps.Application.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Game, GameViewModel>()
                .ReverseMap();
            CreateMap<Map, MapViewModel>()
                .ReverseMap();
            CreateMap<Group, GroupViewModel>()
                .ReverseMap();
            CreateMap<Categorie, CategorieViewModel>()
                .ReverseMap();
            CreateMap<Location, LocationViewModel>()
                .ReverseMap();
            CreateMap<Location, LocationDetailViewModel>()
                .ReverseMap();
            CreateMap<Media, MediaViewModel>()
                .ReverseMap();
            CreateMap<UserMark, UserLocationMarkViewModel>()
                .ReverseMap();
        }
    }
}
