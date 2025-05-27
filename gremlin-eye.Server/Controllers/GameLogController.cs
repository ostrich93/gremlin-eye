using gremlin_eye.Server.Data;
using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace gremlin_eye.Server.Controllers
{
    [Route("api/logs")]
    [ApiController]
    public class GameLogController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public GameLogController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{gameId}")]
        [Authorize(Roles ="Admin,User")]
        public async Task<IActionResult> GetGameLog(long gameId)
        {
            Claim? idClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            Guid? userId = idClaim != null ? Guid.Parse(idClaim!.Value) : null;

            GameLog? gameLog = await _unitOfWork.GameLogs.GetGameLogByUser(gameId, (Guid)userId);
            if (gameLog != null)
            {
                GameLogDTO logData = new GameLogDTO
                {
                    LogId = gameLog.Id,
                    GameId = gameId,
                    IsPlaying = gameLog.IsPlaying,
                    IsBacklog = gameLog.IsBacklog,
                    IsWishlist = gameLog.IsWishlist
                };

                if (gameLog.Playthroughs.Any())
                {
                    Playthrough playthrough = gameLog.Playthroughs.Last();
                    logData.Rating = playthrough.Rating;
                    logData.IsPlayed = gameLog.IsPlayed;
                    logData.PlayStatus = gameLog.PlayStatus;
                }
                return Ok(logData);
            }
            return Ok();
        }
    }
}
