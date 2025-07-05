using Azure.Core;
using gremlin_eye.Server.Data;
using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Entity;
using gremlin_eye.Server.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                        logData.Playthroughs = gameLog.Playthroughs.Select(p => new PlaythroughDTO
                        {
                            LogId = p.GameLogId,
                            PlaythroughId = p.Id,
                            LogTitle = p.LogTitle ?? string.Empty,
                            Medium = p.Medium,
                            IsReplay = p.IsReplay,
                            ReviewText = p.ReviewText,
                            Rating = p.Rating,
                            ContainsSpoilers = p.ReviewSpoilers,
                            ReviewId = p.Review != null ? p.Review.Id : null
                        }).Take(50).ToList();
                    }
                    return Ok(logData);
                }
                else
                {
                    GameData? game = await _unitOfWork.Games.GetGameBySlug(slug);
                    if (game != null)
                    {
                        return Ok(new GameLogDTO
                        {
                            LogId = -1,
                            GameId = game.Id
                        });
                    }
                }
            }
            return Ok();
        }

        [HttpGet("edit/{gameId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetEditGameLog(long gameId)
        {
            Claim? idClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            Guid? userId = idClaim != null ? Guid.Parse(idClaim!.Value) : null;

            if (userId == null) return Unauthorized();

            GameLog? gameLog = await _unitOfWork.GameLogs.GetGameLogByUser(gameId, (Guid)userId);
            GameData? gameData = await _unitOfWork.Games.GetGameById(gameId);

            if (gameData == null) return NotFound();

            GameLogDTO logData;
            if (gameLog != null)
            {
                logData = new GameLogFormDTO
                {
                    LogId = gameLog.Id,
                    GameId = gameLog.GameId,
                    PlayStatus = gameLog.PlayStatus,
                    IsPlayed = gameLog.IsPlayed,
                    IsPlaying = gameLog.IsPlaying,
                    IsBacklog = gameLog.IsBacklog,
                    IsWishlist = gameLog.IsWishlist,
                    GameName = gameData.Name,
                    ReleaseDate = gameData.ReleaseDate,
                    CoverUrl = gameData.CoverUrl,
                    Platforms = gameData.Platforms.Select(p => new PlatformDTO
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Slug = p.Slug
                    }).ToList(),
                    Playthroughs = gameLog.Playthroughs.Count > 0 ?
                        gameLog.Playthroughs.Select(pl => new PlaythroughDTO
                        {
                            LogId = gameLog.Id,
                            GameId = gameId,
                            PlaythroughId = pl.Id,
                            LogTitle = pl.LogTitle != null ? pl.LogTitle : "Log",
                            IsReplay = pl.IsReplay,
                            Medium = pl.Medium,
                            ReviewId = pl.Review != null ? pl.Review.Id : -1,
                            ReviewText = pl.ReviewText,
                            ContainsSpoilers = pl.ReviewSpoilers,
                            Rating = pl.Rating,
                            Platform = pl.Platform != null ? new PlatformDTO
                            {
                                Id = pl.Platform.Id,
                                Name = pl.Platform.Name,
                                Slug = pl.Platform.Slug
                            } : null
                        }).Take(50).ToList() : new List<PlaythroughDTO>([new PlaythroughDTO {
                            GameId = gameId,
                            PlaythroughId = -1,
                            Rating = 0,
                            ReviewId = -1
                        }])
                };
            }
            else
            {
                logData = new GameLogFormDTO
                {
                    LogId = -1,
                    GameId = gameData.Id,
                    GameName = gameData.Name,
                    CoverUrl = gameData.CoverUrl,
                    ReleaseDate = gameData.ReleaseDate,
                    Playthroughs = new List<PlaythroughDTO>([new PlaythroughDTO{
                        GameId = gameId,
                        PlaythroughId = -1,
                        Rating = 0,
                        ReviewId = -1
                    }])
                };
                
            }
            return Ok(logData);
        }

        [HttpPost("{gameId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> SubmitGameLog(long gameId, [FromBody] GameLogDTO request)
        {
            Claim? idClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            Guid? userId = idClaim != null ? Guid.Parse(idClaim!.Value) : null;

            if (userId == null) return Unauthorized();

            GameLog? gameLog = await _unitOfWork.GameLogs.GetGameLogByUser(request.GameId, (Guid)userId);

            if (gameLog != null) //update existing gameLog
            {
                gameLog.PlayStatus = request.PlayStatus;
                gameLog.IsPlayed = request.IsPlayed;
                gameLog.IsPlaying = request.IsPlaying;
                gameLog.IsBacklog = request.IsBacklog;
                gameLog.IsWishlist = request.IsWishlist;

                foreach (PlaythroughDTO playthrough in request.Playthroughs)
                {
                    if (playthrough.PlaythroughId < 0) //New Playthroughs always have a negative id number in request
                    {
                        gameLog.Playthroughs.Add(new Playthrough
                        {
                            //Game = gameLog.Game,
                            GameId = gameId,
                            GameLog = gameLog,
                            GameLogId = gameLog.Id,
                            IsReplay = playthrough.IsReplay,
                            PlatformId = playthrough.Platform!.Id,
                            Rating = playthrough.Rating ?? 0,
                            ReviewText = playthrough.ReviewText,
                            Review = !string.IsNullOrEmpty(playthrough.ReviewText) ? new Review
                            {
                                UserId = (Guid)userId
                            } : null,
                            ReviewSpoilers = playthrough.ContainsSpoilers,
                            LogTitle = playthrough.LogTitle,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow,
                            PlayLogs = playthrough.PlayLogs.Any() ? playthrough.PlayLogs.Select(log => new PlayLog
                            {
                                IsStart = log.IsStart,
                                IsEnd = log.IsEnd,
                                StartDate = log.StartDate,
                                EndDate = log.EndDate,
                                Hours = log.Hours,
                                Minutes = log.Minutes,
                                LogNote = log.LogNote
                            }).ToList() : new List<PlayLog>()
                        });
                    }
                    else //update existing playthrough
                    {
                        Playthrough existingPlaythrough = gameLog.Playthroughs.First(p => p.Id == playthrough.PlaythroughId);
                        existingPlaythrough.IsReplay = playthrough.IsReplay;
                        existingPlaythrough.Rating = playthrough.Rating ?? 0;
                        existingPlaythrough.ReviewText = playthrough.ReviewText;
                        existingPlaythrough.ReviewSpoilers = playthrough.ContainsSpoilers;
                        if (existingPlaythrough.Review == null && !string.IsNullOrEmpty(playthrough.ReviewText))
                        {
                            existingPlaythrough.Review = new Review
                            {
                                UserId = (Guid)userId
                            };
                        }
                        else if (existingPlaythrough.Review != null && string.IsNullOrEmpty(playthrough.ReviewText))
                        {
                            existingPlaythrough.Review = null;
                        }
                        if (playthrough.Platform == null)
                        {
                            existingPlaythrough.Platform = null;
                            existingPlaythrough.PlatformId = null;
                        }
                        else
                        {
                            existingPlaythrough.PlatformId = playthrough.Platform.Id;
                        }
                        existingPlaythrough.LogTitle = playthrough.LogTitle;

                        //check for playLogs
                        foreach (JournalEntryDTO log in playthrough.PlayLogs)
                        {
                            if (log.PlayLogId < 0)
                            {
                                existingPlaythrough.PlayLogs.Add(new PlayLog
                                {
                                    IsStart = log.IsStart,
                                    IsEnd = log.IsEnd,
                                    StartDate = log.StartDate,
                                    EndDate = log.EndDate,
                                    Hours = log.Hours,
                                    Minutes = log.Minutes,
                                    LogNote = log.LogNote
                                });
                            }
                            else
                            {
                                var playLogToEdit = existingPlaythrough.PlayLogs.FirstOrDefault(p => p.Id == log.PlayLogId);
                                if (playLogToEdit != null)
                                {
                                    playLogToEdit.IsStart = log.IsStart;
                                    playLogToEdit.IsEnd = log.IsEnd;
                                    playLogToEdit.Hours = log.Hours;
                                    playLogToEdit.Minutes = log.Minutes;
                                    playLogToEdit.StartDate = log.StartDate;
                                    playLogToEdit.EndDate = log.EndDate;
                                    playLogToEdit.LogNote = log.LogNote;
                                }
                            }
                        }
                        existingPlaythrough.UpdatedAt = DateTime.UtcNow;
                        if (playthrough.PlayLogsToDelete.Count > 0)
                        {
                            await _unitOfWork.Context.PlayLogs.Where(pl => playthrough.PlayLogsToDelete.Contains(pl.Id)).ExecuteDeleteAsync();
                        }
                    }
                    if (request.PlaythroughsToDelete.Count > 0)
                    {
                        await _unitOfWork.Context.Playthroughs.Where(p => request.PlaythroughsToDelete.Contains(p.Id)).ExecuteDeleteAsync();
                    }
                }
                _unitOfWork.GameLogs.UpdateGameLog(gameLog);
                await _unitOfWork.SaveChangesAsync();
                return Ok(gameLog);
            }

            //gameLog is null, so create new one and submit to database.
            gameLog = new GameLog
            {
                GameId = gameId,
                UserId = (Guid)userId,
                PlayStatus = request.PlayStatus,
                IsPlayed = request.IsPlayed,
                IsPlaying = request.IsPlaying,
                IsBacklog = request.IsBacklog,
                IsWishlist = request.IsWishlist
            };
            _unitOfWork.GameLogs.Create(gameLog);

            foreach(PlaythroughDTO play in request.Playthroughs)
            {
                Playthrough playthrough = new Playthrough
                {
                    GameId = gameId,
                    GameLog = gameLog,
                    PlatformId = play.Platform != null ? play.Platform.Id : null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    LogTitle = play.LogTitle,
                    IsReplay = play.IsReplay,
                    ReviewText = play.ReviewText,
                    ReviewSpoilers = play.ContainsSpoilers,
                    Rating = play.Rating != null ? (int)play.Rating : 0,
                    Medium = play.Medium,
                    Review = !string.IsNullOrEmpty(play.ReviewText) ? new Review
                    {
                        UserId = (Guid)userId
                    } : null,
                    PlayLogs = play.PlayLogs.Any() ? play.PlayLogs.Select(l => new PlayLog
                    {
                        StartDate = l.StartDate,
                        EndDate = l.EndDate,
                        IsStart = l.IsStart,
                        IsEnd = l.IsEnd,
                        Hours = l.Hours,
                        Minutes = l.Minutes,
                        LogNote = l.LogNote
                    }).ToList() : new List<PlayLog>()
                };
                gameLog.Playthroughs.Add(playthrough);
            }

            await _unitOfWork.SaveChangesAsync();
            return Ok(gameLog);
        }

        [HttpPost]
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

        [HttpDelete("unlog")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DeleteGameLog([FromBody] UnlogRequest request)
        {
            Claim? idClaim = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            Guid? userId = idClaim != null ? Guid.Parse(idClaim!.Value) : null;

            if (userId == null) return Unauthorized();

            GameLog? gameLog = await _unitOfWork.GameLogs.GetTargetGameLog(request.LogId, (Guid)userId);

            if (gameLog == null)
                return NotFound("The requested game log was not found in the database.");

            _unitOfWork.Context.GameLogs.Remove(gameLog);
            _unitOfWork.Context.SaveChanges();

            return Ok();
        }
    }
}
