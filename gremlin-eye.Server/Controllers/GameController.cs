using gremlin_eye.Server.Data;
using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Entity;
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
        private readonly IUnitOfWork _unitOfWork;

        public GameController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/<GameController>
        [HttpGet("{slug}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGameBySlug(string slug)
        {
            Claim? idClaim = User.Claims.FirstOrDefault(x => x.Type == "UserId");
            Guid? userId = Guid.Parse(idClaim!.Value);

            GameData? data = await _unitOfWork.Games.GetGameBySlug(slug);
            if (data == null)
            {
                return NotFound("The game requested does not exist in our database.");
            }

            int reviewCount = await _unitOfWork.Reviews.GetGameReviewCount(data.Id);
            GameDetailsResponseDTO gameDetails = new GameDetailsResponseDTO
            {
                Id = data.Id,
                Name = data.Name,
                Slug = data.Slug,
                CoverUrl = data.CoverUrl,
                BannerUrl = data.BannerUrl,
                Summary = data.Summary,
                Date = data.ReleaseDate,
                Platforms = data.Platforms.Select(p => new PlatformDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Slug = p.Slug
                }).ToList(),
                Genres = data.Genres.Select(g => new GenreDTO
                {
                    Id = g.Id,
                    Name = g.Name,
                    Slug = g.Slug
                }).ToList(),
                Companies = data.Companies.Select(c => new CompanyDTO
                {
                    Id = c.Id,
                    Name = c.Name,
                    Slug = c.Slug
                }).ToList(),
                Series = data.Series.Count > 0 ? new SeriesDTO
                {
                    Id = data.Series.First().Id,
                    Name = data.Series.First().Name,
                    Slug = data.Series.First().Slug
                } : null,
                ReviewCount = reviewCount
            };

            //GameDetailsResponseDTO gameDetails = await _gameService.GetGameDetailsBySlug(slug, userId);
            GameStatsDTO gameStats = await _unitOfWork.GameLogs.GetGameStats(gameDetails.Id);
            gameStats.AverageRating = _unitOfWork.GameLogs.GetReviewAverage(gameDetails.Id);
            gameStats.RatingCounts = _unitOfWork.GameLogs.GetReviewCounts(gameDetails.Id);
            //GameStatsDTO gameStats = await _logService.GetGameStats(gameDetails.Id);
            gameDetails.PlayedCount = gameStats.PlayedCount;
            gameDetails.PlayingCount = gameStats.PlayingCount;
            gameDetails.BacklogCount = gameStats.BacklogCount;
            gameDetails.WishlistCount = gameStats.WishlistCount;
            gameDetails.RatingCounts = gameStats.RatingCounts;
            gameDetails.AverageRating = gameStats.AverageRating;
            
            if (userId != null) //if requester is a logged in user, then retrieve their game log
            {
                GameLog? gameLog = await _unitOfWork.GameLogs.GetGameLogByUser(gameDetails.Id, (Guid)userId);

                if (gameLog != null)
                {

                    GameLogDTO logData = new GameLogDTO
                    {
                        LogId = gameLog.Id,
                        GameId = gameDetails.Id,
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
                    if (gameLog != null)
                    {

                        gameDetails.GameLog = logData;
                    }
                }
            }

            return Ok(gameDetails);
        }
    }
}
