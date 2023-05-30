using AutoMapper;
using game_maps.Application.IServices;
using game_maps.Application.ViewModels;
using game_maps.Core.Entities;
using game_maps.Infrastructure.IRepositories;
using game_maps.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_maps.Application.Services
{
    public class GameService : IGameService
    {
        private readonly IEfRepository<Game> repository;
        private readonly IMapper mapper;

        public GameService(IEfRepository<Game> repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<GameViewModel>> GetGames()
        {
            var res = await repository.ListAsync();
            return mapper.Map<IEnumerable<GameViewModel>>(res);
        }
    }
}
