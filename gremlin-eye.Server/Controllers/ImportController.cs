using gremlin_eye.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gremlin_eye.Server.Controllers
{
    [Route("/api/import")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly IGameSyncService _gameSyncService;

        public ImportController(IGameSyncService gameSyncService)
        {
            _gameSyncService = gameSyncService;
        }

        [HttpGet("games")]
        [Authorize(Roles = "Admin")]
        public async Task<IResult> ImportGames([FromQuery] int page)
        {
            await _gameSyncService.ImportGames(page);

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            return Results.Ok();
        }

        [HttpGet("genres")]
        [Authorize(Roles = "Admin")]
        public async Task<IResult> ImportGenres()
        {
            await _gameSyncService.ImportGenres();
            return Results.Ok();
        }

        [HttpGet("platforms")]
        [Authorize(Roles = "Admin")]
        public async Task<IResult> ImportPlatforms()
        {
            await _gameSyncService.ImportPlatforms();
            return Results.Ok();
        }

        [HttpGet("series")]
        [Authorize(Roles = "Admin")]
        public async Task<IResult> ImportSeries([FromQuery] int page)
        {
            await _gameSyncService.ImportSeries(page);
            return Results.Ok();
        }

        [HttpGet("companies")]
        [Authorize(Roles = "Admin")]
        public async Task<IResult> ImportCompanies([FromQuery] int page)
        {
            await _gameSyncService.ImportCompanies(page);
            return Results.Ok();
        }
    }
}
