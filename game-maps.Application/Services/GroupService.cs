using AutoMapper;
using game_maps.Application.IServices;
using game_maps.Application.ViewModels.Group;
using game_maps.Core.Entities;
using game_maps.Infrastructure.Contexts;
using game_maps.Infrastructure.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace game_maps.Application.Services
{
    public class GroupService : IGroupService
    {
        private readonly IEfRepository<Group> repository;
        private readonly IEfRepository<Map> mapRepository;
        private readonly IMapper mapper;

        public GroupService(IEfRepository<Group> repository, IEfRepository<Map> mapRepository, IMapper mapper)
        {
            this.repository = repository;
            this.mapRepository = mapRepository;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<GroupViewModel>> GetAll(string slug)
        {
            var map = await mapRepository.SingleAsync(x => x.Slug == slug);
            var groups = await repository.Queryable()
           .Include(x => x.Categories)
           .ThenInclude(x => x.Locations)
           .Where(x => x.Categories.Any(x => x.Locations.Any(x => x.MapId == map.Id)))
           .Select(x => mapper.Map<GroupViewModel>(x))
           .ToListAsync();
            return groups;
        }
    }
}
