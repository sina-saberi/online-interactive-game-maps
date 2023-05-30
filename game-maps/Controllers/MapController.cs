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
            var res = await maps.GetMaps(slug);
            if (res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }

        [HttpGet(nameof(GetLocationsByMapSlug))]
        public async Task<IActionResult> GetLocationsByMapSlug(string slug)
        {
            Guid? userId = User.Identity!.IsAuthenticated ? Guid.Parse(User.Identity!.Name!) : null;
            return Ok(await maps.GetLocations(slug, userId));
        }

        [HttpGet(nameof(GetLocation))]
        public async Task<IActionResult> GetLocation(int id)
        {
            Guid? userId = User.Identity!.IsAuthenticated ? Guid.Parse(User.Identity!.Name!) : null;
            var res = await maps.GetLocationById(id, userId);
            return Ok(res);
        }

        [Authorize]
        [HttpPost(nameof(MarkLocationByUser))]
        public async Task<IActionResult> MarkLocationByUser(UserLocationMarkViewModel model)
        {
            var userId = Guid.Parse(User.Identity!.Name!);
            var res = await maps.MarkLocation(model, userId);
            if (res == null) return BadRequest();
            return Ok(res);
        }
    }
}
