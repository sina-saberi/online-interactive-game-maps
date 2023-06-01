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

        [HttpGet(nameof(GetGroups))]
        public async Task<IActionResult> GetGroups(string slug)
        {
            try
            {
                var res = await service.GetAll(slug);
                return Ok(res);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
