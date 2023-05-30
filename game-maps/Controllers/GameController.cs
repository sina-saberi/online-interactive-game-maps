using game_maps.Application.IServices;
using game_maps.Core.Entities;
using game_maps.Infrastructure.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace game_maps.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService gameService;

        public GameController(IGameService gameService)
        {
            this.gameService = gameService;
        }

        [HttpGet(nameof(GetGames))]
        public async Task<IActionResult> GetGames()
        {
            return Ok(await gameService.GetGames());
        }
    }
}
