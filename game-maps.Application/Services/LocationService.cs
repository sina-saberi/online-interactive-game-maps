using AutoMapper;
using game_maps.Application.IServices;
using game_maps.Application.ViewModels.Location;
using game_maps.Core.Entities;
using game_maps.Infrastructure.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_maps.Application.Services
{
    public class LocationService : ILocationService
    {
        private readonly IEfRepository<Location> repository;
        private readonly IEfRepository<Map> mapRepository;
        private readonly IEfRepository<UserMark> userMarkRepository;
        private readonly IMapper mapper;
        public LocationService(
            IEfRepository<Location> repository,
            IEfRepository<Map> mapRepository,
            IEfRepository<UserMark> userMarkRepository,
            IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.mapRepository = mapRepository;
            this.userMarkRepository = userMarkRepository;
        }

        public async Task<LocationViewModel> AddLocation(AddLocationViewModel model, Guid userId)
        {
            var map = await mapRepository.FirstOrDefaultAsync(x => x.Slug == model.MapSlug);
            if (map != null)
            {
                var location = mapper.Map<Location>(model);
                location.UserId = userId;
                location.Type = Core.Types.LocationType.Location;
                location.MapId = map.Id;
                var res = await repository.CreateAsync(location);
                return mapper.Map<LocationViewModel>(res);
            }
            return null!;
        }
        public async Task<LocationDetailViewModel> Get(int id, Guid? userId)
        {
            return mapper.Map<LocationDetailViewModel>(await repository
                .SingleAsync(
                x => x.Id == id,
                x => x.Medias!,
                x => x.UserMarks!.Where(c => c.UserId == userId)));
        }
        public async Task<IEnumerable<LocationViewModel>> GetAll(string slug, Guid? userId)
        {
            var map = await mapRepository.SingleAsync(x => x.Slug == slug);
            var locations = await repository.WhereAsync(x =>
            x.MapId == map.Id
            && x.UserId != null ? x.UserId == userId : true,
                 x => x.Categorie!,
                 x => x.UserMarks!.Where(c => c.UserId == userId));
            return mapper.Map<IEnumerable<LocationViewModel>>(locations);
        }
        public async Task<UserLocationMarkViewModel> MarkLocation(UserLocationMarkViewModel model, Guid userId)
        {
            UserMark res;
            var marker = await userMarkRepository.FirstOrDefaultAsync(x => x.UserId == userId && x.LocationId == model.LocationId);
            if (marker is not null)
            {
                marker.IsDone = model.IsDone;
                res = await userMarkRepository.UpdateAsync(marker);
            }
            else
                res = await userMarkRepository.CreateAsync(new UserMark
                {
                    IsDone = model.IsDone,
                    LocationId = model.LocationId,
                    UserId = userId,
                });
            return mapper.Map<UserLocationMarkViewModel>(res);
        }
    }
}
