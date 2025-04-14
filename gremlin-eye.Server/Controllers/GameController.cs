using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Interfaces.Services;
using gremlin_eye.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace gremlin_eye.Server.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;
        private readonly IGameLogService _logService;

        public GameController(IGameService gameService, IGameLogService logService)
        {
            _gameService = gameService;
            _logService = logService;
        }

        // GET: api/<GameController>
        [HttpGet("{slug}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGameBySlug(string slug)
        {
            Claim? idClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            long? userId = long.Parse(idClaim!.Value);

            GameDetailsResponseDTO gameDetails = await _gameService.GetGameDetailsBySlug(slug, userId);
            GameStatsDTO gameStats = await _logService.GetGameStats(gameDetails.Id);
            gameDetails.PlayedCount = gameStats.PlayedCount;
            gameDetails.PlayingCount = gameStats.PlayingCount;
            gameDetails.BacklogCount = gameStats.BacklogCount;
            gameDetails.WishlistCount = gameStats.WishlistCount;
            gameDetails.RatingCounts = gameStats.RatingCounts;
            gameDetails.AverageRating = gameStats.AverageRating;
            
            if (userId != null) //if requester is a logged in user, then retrieve their game log
            {
                GameLogDTO? gameLog = await _logService.GetGameLogByUser(gameDetails.Id, (long)userId);
                if (gameLog != null)
                {
                    gameDetails.GameLog = gameLog;
                }
            }

            return Ok(gameDetails);
        }
    }
}
