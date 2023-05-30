using AutoMapper;
using game_maps.Application.IServices;
using game_maps.Application.ViewModels;
using game_maps.Core.Entities;
using game_maps.Infrastructure.Contexts;
using game_maps.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;


namespace game_maps.Application.Services
{
    public class MapService : IMapService
    {
        private readonly IEfRepository<Game> gameRepository;
        private readonly IEfRepository<Map> repository;
        private readonly IEfRepository<Location> locationRepository;
        private readonly IEfRepository<UserMark> userMarkRepository;
        private readonly IMapper mapper;

        public MapService(
            IEfRepository<Game> gameRepository,
            IEfRepository<Map> repository,
            IEfRepository<Location> locationRepository,
            IEfRepository<UserMark> userMarkRepository,
            IMapper mapper)
        {
            this.gameRepository = gameRepository;
            this.repository = repository;
            this.locationRepository = locationRepository;
            this.userMarkRepository = userMarkRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<LocationViewModel>> GetLocations(string slug, Guid? userId)
        {
            var map = await repository.FirstOrDefaultAsync(x => x.Slug == slug);
            if (map is not null)
            {
                var res = await locationRepository.Queryable().Where(x => x.MapId == map.Id)
                    .Include(x => x.Categorie)
                    .Include(x => x.UserMarks!.Where(x => x.UserId == userId))
                    .ToListAsync();

                return res.Select(x => new LocationViewModel()
                {
                    CategorieName = x.Categorie != null ? x.Categorie.Title : null,
                    Icon = x.Categorie != null ? x.Categorie.Icon : null,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    Id = x.Id,
                    IsDone = x.UserMarks != null ? x.UserMarks.Any(x => x.IsDone) : false
                });
            }
            return null!;
        }
        public async Task<IEnumerable<MapViewModel>> GetMaps(string slug)
        {
            var game = await gameRepository.FirstOrDefaultAsync(x => x.Slug == slug);
            if (game is not null)
            {
                return mapper.Map<IEnumerable<MapViewModel>>(await repository.WhereAsync(x => x.GameId == game.Id));
            }
            return null!;
        }
        public async Task<LocationDetailViewModel> GetLocationById(int id, Guid? userId)
        {
            var res = await locationRepository.Queryable()
                .Include(x => x.Medias)
                .Include(x => x.UserMarks!.Where(x => x.UserId == userId))
                .SingleAsync(x => x.Id == id);

            return new LocationDetailViewModel()
            {
                Title = res.Title,
                Description = res.Description,
                Features = res.Features,
                IgnMarkerId = res.IgnMarkerId,
                IgnPageId = res.IgnPageId,
                Medias = mapper.Map<ICollection<MediaViewModel>>(res.Medias),
                Marker = res.UserMarks!.Any() ? mapper.Map<UserLocationMarkViewModel>(res.UserMarks!.Last()) : null,
            };
        }
        public async Task<UserLocationMarkViewModel> MarkLocation(UserLocationMarkViewModel model, Guid userId)
        {

            UserMark Marker = mapper.Map<UserMark>(model);
            Marker.UserId = userId;
            if (model.Id != null)
            {
                Marker = await userMarkRepository.SingleAsync(x => x.Id == model.Id);
                Marker.LastEditDate = DateTime.Now;
                Marker.IsDone = model.IsDone;
                Marker.Note = model.Note;
                Marker = await userMarkRepository.UpdateAsync(Marker);
            }
            else
            {
                Marker.LastEditDate = DateTime.Now;
                Marker = await userMarkRepository.CreateAsync(Marker);
            }
            return mapper.Map<UserLocationMarkViewModel>(Marker);
        }
    }
}
