﻿using gremlin_eye.Server.Data;
using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Entity;
using gremlin_eye.Server.Enums;
using LinqKit;
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
            GameData? data = await _unitOfWork.Games.GetGameBySlug(slug);
            if (data == null)
            {
                return NotFound("The game requested does not exist in our database.");
            }

            var topReviews = await _unitOfWork.Reviews.GetGameTopReviews(data.Id, data.Slug, data.Name);
            int reviewCount = await _unitOfWork.Reviews.GetGameReviewCount(data.Id);
            int likeCount = _unitOfWork.Likes.GetGameLikeCount(data.Id);
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
                TopReviews = topReviews,
                ReviewCount = reviewCount,
                LikeCount = likeCount
            };

            //GameDetailsResponseDTO gameDetails = await _gameService.GetGameDetailsBySlug(slug, userId);
            GameStatsDTO gameStats = await _unitOfWork.GameLogs.GetGameStats(gameDetails.Id);
            gameStats.AverageRating = _unitOfWork.GameLogs.GetReviewAverage(gameDetails.Id);
            RatingCount[] rCounts = _unitOfWork.GameLogs.GetRatingCounts(gameDetails.Id);
            if (rCounts.Any())
            {
                foreach (RatingCount rCount in rCounts)
                {
                    gameStats.RatingCounts[rCount.Rating - 1] = rCount.Count;
                }
            }
            //gameStats.RatingCounts = _unitOfWork.GameLogs.GetReviewCounts(gameDetails.Id);
            //GameStatsDTO gameStats = await _logService.GetGameStats(gameDetails.Id);
            gameDetails.Stats = gameStats;

            long seriesId = gameDetails.Series != null ? gameDetails.Series.Id : -1;
            if (seriesId > -1) {
                List<GameData>? relatedGames = await _unitOfWork.Games.GetRelatedGames(seriesId, gameDetails.Id);
                if (relatedGames != null)
                {
                    List<GameSummaryDTO> relatedSummaries = new List<GameSummaryDTO>();
                    foreach (var relatedGame in relatedGames)
                    {
                        relatedSummaries.Add(new GameSummaryDTO
                        {
                            Id = relatedGame.Id,
                            Slug = relatedGame.Slug,
                            Name = relatedGame.Name,
                            CoverUrl = relatedGame.CoverUrl,
                            ReleaseDate = relatedGame.ReleaseDate
                        });
                    }
                    gameDetails.RelatedGames = relatedSummaries;
                }
            }

            return Ok(gameDetails);
        }

        [HttpGet("quick_search")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSuggestions([FromQuery] string query)
        {
            var searchResults = await _unitOfWork.Games.SearchGames(query);
            var suggestions = new List<GameSuggestionDTO>();
            foreach (GameData game in searchResults)
            {
                suggestions.Add(new GameSuggestionDTO
                {
                    Id = game.Id,
                    Value = game.Name,
                    Slug = game.Slug,
                    Year = game.ReleaseDate != null ? game.ReleaseDate.Value.Year : -1
                });
            }
            return Ok(suggestions);
        }

        [HttpPost("rate/{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> RateGame(long id, [FromBody] RatingRequestDTO rating)
        {
            Claim idClaim = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);
            Guid userId = Guid.Parse(idClaim!.Value);

            var user = _unitOfWork.Users.GetUserById(userId);
            if (user == null)
                return Unauthorized("User not found");

            var game = await _unitOfWork.Games.GetGameById(id);
            if (game == null)
                return NotFound("Game not found");

            var gameLog = await _unitOfWork.GameLogs.GetGameLogByUser(id, userId);

            if (gameLog != null)
            {
                var playthrough = gameLog.Playthroughs.LastOrDefault();
                if (playthrough != null)
                {
                    playthrough.Rating = rating.Rating;
                    if (!gameLog.IsPlayed)
                    {
                        gameLog.IsPlayed = true;
                        gameLog.PlayStatus = PlayState.Played;
                    }
                }
                else
                {
                    gameLog.Playthroughs.Add(new Playthrough
                    {
                        Game = game,
                        GameId = id,
                        GameLog = gameLog,
                        GameLogId = gameLog.Id,
                        Rating = rating.Rating
                    });
                }
                _unitOfWork.Context.GameLogs.Update(gameLog);
            }
            else
            {
                gameLog = new GameLog
                {
                    UserId = userId,
                    User = user,
                    GameId = id,
                    IsPlayed = true,
                    PlayStatus = PlayState.Played
                };

                _unitOfWork.GameLogs.Create(gameLog);

                var playthrough = new Playthrough
                {
                    GameLogId = gameLog.Id,
                    GameLog = gameLog,
                    GameId = id,
                    Game = game,
                    Rating = rating.Rating
                };

                gameLog.Playthroughs.Add(playthrough);
            }

            await _unitOfWork.SaveChangesAsync();
            return Ok(gameLog.Id);
        }

        [HttpPost("log")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> LogGame(GameLogDTO gameLogState)
        {
            Claim? idClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            Guid? userId = Guid.Parse(idClaim!.Value);

            var user = _unitOfWork.Users.GetUserById((Guid)userId);
            if (user == null)
                return NotFound("User not found");


            var gameLog = await _unitOfWork.GameLogs.GetGameLogByUser(gameLogState.GameId, (Guid)userId);

            if (gameLog != null)
            {
                gameLog.PlayStatus = gameLogState.PlayStatus;
                gameLog.IsPlayed = gameLogState.IsPlayed;
                gameLog.IsPlaying = gameLogState.IsPlaying;
                gameLog.IsBacklog = gameLogState.IsBacklog;
                gameLog.IsWishlist = gameLogState.IsWishlist;

                _unitOfWork.Context.GameLogs.Update(gameLog);
            }
            else
            {
                gameLog = new GameLog
                {
                    User = user,
                    UserId = (Guid)userId,
                    GameId = gameLogState.GameId,
                    PlayStatus = gameLogState.PlayStatus,
                    IsPlayed = gameLogState.IsPlayed,
                    IsPlaying = gameLogState.IsPlaying,
                    IsBacklog = gameLogState.IsBacklog,
                    IsWishlist = gameLogState.IsWishlist
                };

                _unitOfWork.GameLogs.Create(gameLog);

                if (gameLogState.Rating != null)
                {
                    await _unitOfWork.SaveChangesAsync();
                    Playthrough playthrough = new Playthrough
                    {
                        GameId = gameLog.GameId,
                        GameLog = gameLog,
                        GameLogId = gameLog.Id,
                        Rating = (int)gameLogState.Rating
                    };
                    gameLog.Playthroughs.Add(playthrough);
                }
            }

            await _unitOfWork.SaveChangesAsync();
            return Ok(gameLog.Id);
        }

        [HttpGet("lib")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGameList(
            [FromQuery] string? releaseYear, [FromQuery] string? genre, [FromQuery] string? category, [FromQuery] string? platform,
            [FromQuery] double min = 0, [FromQuery] double max = 5,
            [FromQuery] string orderBy = Constants.ORDER_TRENDING, [FromQuery] string sortOrder = Constants.DESC,
            [FromQuery] int page = 1)
        {
            var predicate = PredicateBuilder.New<GameData>();

            if (releaseYear != null)
            {
                switch (releaseYear)
                {
                    case Constants.UPCOMING:
                        predicate = predicate.And(g => g.ReleaseDate == null || g.ReleaseDate > DateTimeOffset.UtcNow);
                        break;
                    case Constants.RELEASED:
                        predicate = predicate.And(g => g.ReleaseDate != null && g.ReleaseDate <= DateTimeOffset.UtcNow);
                        break;
                    default:
                        if (releaseYear.Trim().Length == 0)
                            break;
                        int yearStr;
                        if (int.TryParse(releaseYear, out yearStr))
                        {
                            predicate.And(g => g.ReleaseDate.HasValue && g.ReleaseDate.Value.Year == yearStr);
                        }
                        break;
                }
            }
            if (genre != null)
                predicate.And(g => g.Genres.Any(gen => gen.Slug == genre));

            if (category != null)
                predicate.And(g => g.GameType == category);

            if (platform != null)
                predicate.And(g => g.Platforms.Any(p => p.Slug == platform));

            var inner = PredicateBuilder.New<GameData>();
            inner.And(g => g.Playthroughs.Select(p => p.Rating).DefaultIfEmpty().Average() >= 2 * min);
            inner.And(g => g.Playthroughs.Select(p => p.Rating).DefaultIfEmpty().Average() <= 2 * max);

            predicate.And(inner);

            var paginatedList = await _unitOfWork.Games.GetPaginatedList(predicate, orderBy, sortOrder, page);
            return Ok(paginatedList);
        }

        [HttpGet("user/{username}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserGames(string username, [FromQuery] string? releaseYear, [FromQuery] string? genre, [FromQuery] string? releasePlatform,
            [FromQuery] string? playedPlatform, [FromQuery] string? playStatus, [FromQuery(Name = "playTypes[]")] string[] playTypes,
            [FromQuery] int rating = 0, [FromQuery] string orderBy = Constants.ORDER_TRENDING, [FromQuery] string sortOrder = Constants.DESC,
            [FromQuery] int page = 1)
        {
            var user = await _unitOfWork.Users.GetUserByNameAsync(username);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var predicate = PredicateBuilder.New<GameData>();

            if (releaseYear != null)
            {
                switch (releaseYear)
                {
                    case Constants.UPCOMING:
                        predicate = predicate.And(g => g.ReleaseDate == null || g.ReleaseDate > DateTimeOffset.UtcNow);
                        break;
                    case Constants.RELEASED:
                        predicate = predicate.And(g => g.ReleaseDate != null && g.ReleaseDate <= DateTimeOffset.UtcNow);
                        break;
                    default:
                        if (releaseYear.Trim().Length == 0)
                            break;
                        int yearStr;
                        if (int.TryParse(releaseYear, out yearStr))
                        {
                            predicate.And(g => g.ReleaseDate.HasValue && g.ReleaseDate.Value.Year == yearStr);
                        }
                        break;
                }
            }
            if (genre != null)
                predicate.And(g => g.Genres.Any(gen => gen.Slug == genre));
            if (releasePlatform != null)
                predicate.And(g => g.Platforms.Any(p => p.Slug == releasePlatform));
            if (playedPlatform != null)
                predicate.And(g => g.GameLogs.Any(l => l.UserId == user.Id && l.Playthroughs.Where(p => p.Platform != null && p.Platform.Slug == playedPlatform).Any()));
            if (playStatus != null)
                predicate.And(g => g.GameLogs.Any(l => l.UserId == user.Id && l.PlayStatus.HasValue && ((PlayState)l.PlayStatus).ToStringValue() == playStatus));

            predicate.And(g => g.GameLogs.Any(l => l.UserId == user.Id && l.Playthroughs.Count(p => p.Rating >= rating) > 0));

            var inner = PredicateBuilder.New<GameData>();
            foreach(string playType in playTypes)
            {
                if (PlayingType.Played.ToStringValue() == playType)
                    inner.Or(g => g.GameLogs.Any(l => l.UserId == user.Id && l.IsPlayed));
                else if (PlayingType.Playing.ToStringValue() == playType)
                    inner.Or(g => g.GameLogs.Any(l => l.UserId == user.Id && l.IsPlaying));
                else if (PlayingType.Backlog.ToStringValue() == playType)
                    inner.Or(g => g.GameLogs.Any(l => l.UserId == user.Id && l.IsBacklog));
                else if (PlayingType.Wishlist.ToStringValue() == playType)
                    inner.Or(g => g.GameLogs.Any(l => l.UserId == user.Id && l.IsWishlist));
            }
            predicate.And(inner);

            var paginatedList = await _unitOfWork.Games.GetPaginatedList(predicate, user.Id, orderBy, sortOrder, page, 40);

            return Ok(paginatedList);
        }

        [HttpGet("reviews/{slug}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGameReviews(string slug, [FromQuery] int page = 1)
        {
            var gameData = await _unitOfWork.Games.GetGameBySlug(slug);
            if (gameData == null)
            {
                return NotFound("Game not found");
            }

            var reviews = await _unitOfWork.Reviews.GetGameReviews(gameData.Id, slug, gameData.Name, page);
            return Ok(new
            {
                GameName = gameData.Name,
                gameData.CoverUrl,
                Reviews = reviews,
                IsEnd = reviews.Count < 32 //if the total number of reviews returned is less than 
            });
        }
    }
}
