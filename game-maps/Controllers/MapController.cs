using game_maps.Application.IServices;
using game_maps.Application.ViewModels;
using game_maps.Core.Entities;
using game_maps.Infrastructure.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace game_maps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly IMapService maps;

        public MapController(IMapService maps)
        {
            this.maps = maps;
        }

        [HttpGet(nameof(GetMaps))]
        public async Task<IActionResult> GetMaps(string slug)
        {
            var res = await maps.GetAll(slug);
            return Ok(res);
        }

        [HttpGet(nameof(GetMap))]
        public async Task<IActionResult> GetMap(string slug)
        {
            try
            {
                var res = await maps.Get(slug);
                return Ok(res);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
