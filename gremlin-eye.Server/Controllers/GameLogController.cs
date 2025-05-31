using gremlin_eye.Server.Data;
using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Entity;
using gremlin_eye.Server.Enums;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("{slug}")]
        [Authorize(Roles ="Admin,User")]
        public async Task<IActionResult> GetGameLog(string slug)
        {
            Claim? idClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            Guid? userId = idClaim != null ? Guid.Parse(idClaim!.Value) : null;

            if (userId != null)
            {
                GameLog? gameLog = await _unitOfWork.GameLogs.GetGameLogByUser(slug, (Guid)userId);
                if (gameLog != null)
                {
                    GameLogDTO logData = new GameLogDTO
                    {
                        LogId = gameLog.Id,
                        GameId = gameLog.GameId,
                        PlayStatus = gameLog.PlayStatus,
                        IsPlayed = gameLog.IsPlayed,
                        IsPlaying = gameLog.IsPlaying,
                        IsBacklog = gameLog.IsBacklog,
                        IsWishlist = gameLog.IsWishlist
                    };

                    if (gameLog.Playthroughs.Any())
                    {
                        Playthrough playthrough = gameLog.Playthroughs.Last();
                        logData.Rating = playthrough.Rating;
                    }
                    return Ok(logData);
                }
            }
            return Ok();
        }

        [HttpPost("status")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> UpdateGameLog([FromBody] UpdateGameLogTypeRequest request)
        {
            Claim? idClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            Guid? userId = idClaim != null ? Guid.Parse(idClaim!.Value) : null;

            if (userId == null) return Unauthorized();

            GameLog? gameLog = await _unitOfWork.GameLogs.GetGameLogByUser(request.GameId, (Guid)userId);
            if (gameLog != null)
            {
                bool changed = false;
                switch (request.Type)
                {
                    case PlayingType.Played:
                        gameLog.IsPlayed = !gameLog.IsPlayed;
                        if (!gameLog.IsPlayed)
                            gameLog.PlayStatus = null;
                        changed = true;
                        break;
                    case PlayingType.Playing:
                        gameLog.IsPlaying = !gameLog.IsPlaying;
                        changed = true;
                        break;
                    case PlayingType.Backlog:
                        gameLog.IsBacklog = !gameLog.IsBacklog;
                        changed = true;
                        break;
                    case PlayingType.Wishlist:
                        gameLog.IsWishlist = !gameLog.IsWishlist;
                        changed = true;
                        break;
                    default:
                        break;
                }
                if (changed)
                {
                    _unitOfWork.Context.GameLogs.Update(gameLog);
                    await _unitOfWork.SaveChangesAsync();
                    return Ok(gameLog.Id);
                }
            }
            else
            {
                //var game = await _unitOfWork.Games.GetGameById(gameId);
                var user = _unitOfWork.Users.GetUserById((Guid)userId);
                if (user != null) {
                    gameLog = new GameLog
                    {
                        GameId = request.GameId,
                        User = user,
                        UserId = user.Id,
                        PlayStatus = request.Type == PlayingType.Played ? PlayState.Played : null,
                        IsPlayed = request.Type == PlayingType.Played,
                        IsPlaying = request.Type == PlayingType.Playing,
                        IsBacklog = request.Type == PlayingType.Backlog,
                        IsWishlist = request.Type == PlayingType.Wishlist
                    };
                    _unitOfWork.GameLogs.Create(gameLog);
                    await _unitOfWork.SaveChangesAsync();
                    return Ok(gameLog.Id);
                }
            }

            return BadRequest("A game log could not be found or created.");
        }

        [HttpPatch("status")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> UpdatePlayStatus([FromBody] UpdatePlayStatusRequest request)
        {
            Claim? idClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            Guid? userId = idClaim != null ? Guid.Parse(idClaim!.Value) : null;

            if (userId == null) return Unauthorized();

            var gameLog = await _unitOfWork.GameLogs.GetGameLogByUser(request.GameId, (Guid)userId);
            if (gameLog == null) return NotFound();

            gameLog.PlayStatus = request.Status;
            _unitOfWork.Context.GameLogs.Update(gameLog);
            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }
    }
}
