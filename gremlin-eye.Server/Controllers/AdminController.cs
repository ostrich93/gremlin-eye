using gremlin_eye.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace gremlin_eye.Server.Controllers
{
    [Route("/api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IGameSyncService _gameSyncService;

        public AdminController(IGameSyncService gameSyncService)
        {
            _gameSyncService = gameSyncService;
        }

        [HttpGet("import")]
        [Authorize(Roles = "Admin")]
        public async Task Import([FromQuery] int page)
        {
            await _gameSyncService.Import(page);
        }
    }
}
