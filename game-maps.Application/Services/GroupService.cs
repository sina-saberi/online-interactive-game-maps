using AutoMapper;
using game_maps.Application.IServices;
using game_maps.Application.ViewModels;
using game_maps.Core.Entities;
using game_maps.Infrastructure.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_maps.Application.Services
{
    public class GroupService : IGroupService
    {
        private readonly IEfRepository<Group> repository;
        private readonly IEfRepository<Game> gameRepository;
        private readonly IMapper mapper;

        public GroupService(IEfRepository<Group> repository, IEfRepository<Game> gameRepository, IMapper mapper)
        {
            this.repository = repository;
            this.gameRepository = gameRepository;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<GroupViewModel>> GetGroups(string slug)
        {
            var game = await gameRepository.FirstOrDefaultAsync(x => x.Slug == slug);
            if (game is not null)
            {
                var res = await repository.WhereAsync(x => x.GameId == game.Id, i => i.Categories!);
                return mapper.Map<IEnumerable<GroupViewModel>>(res);
            }
            return null!;
        }
    }
}
