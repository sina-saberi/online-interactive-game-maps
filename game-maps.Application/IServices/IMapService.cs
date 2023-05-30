﻿using game_maps.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_maps.Application.IServices
{
    public interface IMapService
    {
        public Task<IEnumerable<MapViewModel>> GetMaps(string slug);

        public Task<IEnumerable<LocationViewModel>> GetLocations(string slug,Guid? userId);

        public Task<LocationDetailViewModel> GetLocationById(int id,Guid? userId);

        public Task<UserLocationMarkViewModel> MarkLocation(UserLocationMarkViewModel model,Guid userId);
    }
}