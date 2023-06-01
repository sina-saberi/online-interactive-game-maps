using AutoMapper;
using game_maps.Application.IServices;
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
using game_maps.Application.ViewModels.Map;

namespace game_maps.Application.Services
{
    public class MapService : IMapService
    {
        private readonly IEfRepository<Map> repository;
        private readonly IEfRepository<Game> gameRepository;
        private readonly IMapper mapper;
        public MapService(
            IEfRepository<Map> repository,
            IEfRepository<Game> gameRepository,
            IMapper mapper)
        {
            this.repository = repository;
            this.gameRepository = gameRepository;
            this.mapper = mapper;
        }
        public async Task<MapDetailViewModel> Get(string key)
        {
            return mapper.Map<MapDetailViewModel>(await repository.SingleAsync(x => x.Slug == key));
        }
        public async Task<IEnumerable<MapViewModel>> GetAll(string slug)
        {
            var game = await gameRepository.FirstOrDefaultAsync(x => x.Slug == slug);
            if (game is not null)
            {
                return mapper.Map<IEnumerable<MapViewModel>>(await repository.WhereAsync(x => x.GameId == game.Id));
            }
            return null!;
        }
    }
}
