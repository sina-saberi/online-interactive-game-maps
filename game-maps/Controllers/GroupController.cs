using game_maps.Application.IServices;
using game_maps.Core.Entities;
using game_maps.Infrastructure.IRepositories;
using Microsoft.AspNetCore.Mvc;

namespace game_maps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService service;

        public GroupController(IGroupService service)
        {
            this.service = service;
        }

        [HttpGet(nameof(GetGroupsWithCategoriesByGameSlug))]
        public async Task<IActionResult> GetGroupsWithCategoriesByGameSlug(string slug)
        {
            var res = await service.GetGroups(slug);
            if (res is not null)
                return Ok(res);
            return NotFound();
        }
    }
}
