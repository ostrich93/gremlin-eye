using gremlin_eye.Server.Comparers;
using gremlin_eye.Server.Data;
using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Entity;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace gremlin_eye.Server.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly DataContext _context;

        public GameRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<GameData[]> GetGames(int offset, int limit)
        {
            return await _context.Games.Skip(offset * limit).OrderBy(g => g.Id).ToArrayAsync();
        }

        public async Task<GameData?> GetGameById(long id)
        {
            return await _context.Games
                .Include(g => g.GameCompanies)
                    .ThenInclude(gc => gc.Company)
                .Include(g => g.GameGenres)
                    .ThenInclude(gg => gg.Genre)
                .Include(g => g.GamePlatforms)
                    .ThenInclude(gp => gp.Platform)
                .Include(g => g.GameSeries)
                    .ThenInclude(gs => gs.Series)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task CreateAndSaveAsync(GameData game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
        }

        public async Task<GameData?> GetGameBySlug(string slug)
        {
            return await _context.Games
                .Include(g => g.GameCompanies)
                    .ThenInclude(gc => gc.Company)
                .Include(g => g.GameGenres)
                    .ThenInclude(gg => gg.Genre)
                .Include(g => g.GamePlatforms)
                    .ThenInclude(gp => gp.Platform)
                .Include(g => g.GameSeries)
                    .ThenInclude(gs => gs.Series)
                .FirstOrDefaultAsync(g => g.Slug == slug);
        }

        public void Create(GameData data)
        {
            _context.Games.Add(data);
        }

        public async Task CreateRangeAndSaveAsync(IEnumerable<GameData> data)
        {
            _context.Games.AddRange(data);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRangeAndSaveAsync(IEnumerable<GameData> data)
        {
            _context.Games.UpdateRange(data);
            await _context.SaveChangesAsync();
        }

        public async Task<GameData[]> SearchGames(string query)
        {
            return await _context.Games.Where(g => g.Name.Contains(query)).Take(50).ToArrayAsync();
        }

        public async Task<List<GameData>?> GetRelatedGames(long seriesId, long gameId)
        {
            var gs = await _context.GameSeries.Where(gsr => gsr.SeriesId == seriesId && gameId == gsr.GameId).OrderByDescending(gsr => gsr.Game.ReleaseDate).ToListAsync();
            if (gs == null)
                return null;

            List<GameData> relatedGames = new List<GameData>();
            var idx = gs.FindIndex(g => g.GameId == gameId);
            if (idx < 3)
            {
                foreach(GameSeries gd in gs)
                {
                    if (gd.GameId == gameId)
                        continue;
                    relatedGames.Add(gd.Game);
                }
            }
            else if (idx == gs.Count - 1) //last entry
            {
                int startIdx = Math.Max(idx - 6, 0);
                int cIdx = startIdx;
                while (cIdx < gs.Count)
                {
                    if (gs[cIdx].GameId != gameId)
                        relatedGames.Add(gs[cIdx].Game);
                    cIdx++;
                }
            }
            else if (idx == gs.Count - 2) //second to last entry
            {
                int startIdx = Math.Max(idx - 5, 0);
                int cIdx = startIdx;
                while (cIdx < gs.Count)
                {
                    if (gs[cIdx].GameId != gameId)
                        relatedGames.Add(gs[cIdx].Game);
                    cIdx++;
                }
            }
            else
            {
                int startIdx = Math.Max(idx - 3, 0);
                int endIdx = Math.Min(idx + 1, gs.Count);
                int currIdx = startIdx;
                while (currIdx < endIdx)
                {
                    if (gs[currIdx].GameId != gameId)
                        relatedGames.Add(gs[currIdx].Game);
                    currIdx++;
                }
            }

            return relatedGames;
        }

        public async Task<PaginatedList<GameSummaryDTO>> GetFilteredResults(string? releaseYear, string? genre, string? platform, double min, double max, string orderBy, string sortOrder, int page)
        {
            var gamesQuery = _context.Games.AsNoTracking()
                .Select(g =>
                new {
                    Id = g.Id,
                    Name = g.Name,
                    Slug = g.Slug,
                    ReleaseDate = g.ReleaseDate,
                    CoverUrl = g.CoverUrl,
                    GameGenres = g.GameGenres,
                    GamePlatforms = g.GamePlatforms,
                    AverageRating = g.Playthroughs.Any() ? g.Playthroughs.Average(p => p.Rating) : 0
                })
                .AsQueryable();

            if (!string.IsNullOrEmpty(releaseYear))
            {
                switch (releaseYear) {
                    case Constants.UPCOMING:
                        gamesQuery = gamesQuery.Where(g => g.ReleaseDate == null || g.ReleaseDate > DateTimeOffset.UtcNow);
                        break;
                    case Constants.RELEASED:
                        gamesQuery = gamesQuery.Where(g => g.ReleaseDate != null && g.ReleaseDate <= DateTimeOffset.UtcNow);
                        break;
                    default:
                        if (releaseYear.Trim().Length == 0)
                            break;
                        int yearStr;
                        if (int.TryParse(releaseYear, out yearStr))
                        {
                            gamesQuery = gamesQuery.Where(g => g.ReleaseDate.HasValue && yearStr == g.ReleaseDate.Value.Year);
                        }
                        break;
                }
            }

            if (!string.IsNullOrEmpty(genre))
                gamesQuery = gamesQuery.Where(g => g.GameGenres.Any(gg => string.Equals(genre, gg.Genre.Slug)));
            if (!string.IsNullOrEmpty(platform))
                gamesQuery = gamesQuery.Where(g => g.GamePlatforms.Any(gp => string.Equals(platform, gp.Platform.Slug)));

            if (min > 0)
                gamesQuery = gamesQuery.Where(g => g.AverageRating >= 2 * min);
            if (max < 5)
                gamesQuery = gamesQuery.Where(g => g.AverageRating <= 2 * max);

            int totalItems = await gamesQuery.AsSplitQuery().CountAsync();

            switch (orderBy)
            {
                case Constants.ORDER_RELEASE_DATE:
                    gamesQuery = sortOrder == Constants.ASC ?
                        gamesQuery.OrderBy(g => g.ReleaseDate).ThenBy(g => g.Slug).Skip((page - 1) * Constants.PAGE_LIMIT_A).Take(Constants.PAGE_LIMIT_A) :
                        gamesQuery.OrderByDescending(g => g.ReleaseDate).ThenBy(g => g.Slug).Skip((page - 1) * Constants.PAGE_LIMIT_A).Take(Constants.PAGE_LIMIT_A);
                    break;
                case Constants.ORDER_GAME_TITLE:
                    gamesQuery = sortOrder == Constants.ASC ?
                        gamesQuery.OrderBy(g => g.Slug).ThenBy(g => g.Id).Skip((page - 1) * Constants.PAGE_LIMIT_A).Take(Constants.PAGE_LIMIT_A) :
                        gamesQuery.OrderByDescending(g => g.Slug).ThenBy(g => g.Id).Skip((page - 1) * Constants.PAGE_LIMIT_A).Take(Constants.PAGE_LIMIT_A);
                    break;
                case Constants.ORDER_GAME_RATING:
                    gamesQuery = sortOrder == Constants.ASC ?
                        gamesQuery.OrderBy(g => g.AverageRating).ThenBy(g => g.Slug).Skip((page - 1) * Constants.PAGE_LIMIT_A).Take(Constants.PAGE_LIMIT_A) :
                        gamesQuery.OrderByDescending(g => g.AverageRating).ThenBy(g => g.Slug).Skip((page - 1) * Constants.PAGE_LIMIT_A).Take(Constants.PAGE_LIMIT_A);
                    break;
                default:
                    gamesQuery = sortOrder == Constants.ASC ?
                        gamesQuery.OrderBy(g => g.Id).Skip((page - 1) * Constants.PAGE_LIMIT_A).Take(Constants.PAGE_LIMIT_A) :
                        gamesQuery.OrderByDescending(g => g.Id).Skip((page - 1) * Constants.PAGE_LIMIT_A).Take(Constants.PAGE_LIMIT_A);
                    break;
            }

            var resultItems = await gamesQuery.AsSplitQuery().ToListAsync();

            return new PaginatedList<GameSummaryDTO>
            {
                Items = resultItems.Select(item => new GameSummaryDTO
                {
                    Id = item.Id,
                    Name = item.Name,
                    Slug = item.Slug,
                    CoverUrl = item.CoverUrl,
                    ReleaseDate = item.ReleaseDate
                }).ToList(),
                TotalItems = totalItems,
                PageNumber = page,
                PageLimit = Constants.PAGE_LIMIT_A
            };
        }

        public async Task<PaginatedList<GameSummaryDTO>> GetPaginatedList(ExpressionStarter<GameData> predicate, string orderBy, string sortOrder, int page = 1, int itemsPerPage = Constants.PAGE_LIMIT_A )
        {
            var totalItems = await _context.Games.AsNoTracking().CountAsync(predicate);

            List<GameData> games;
            switch (orderBy)
            {
                case Constants.ORDER_RELEASE_DATE:
                    games = sortOrder == Constants.ASC ? await _context.Games.AsNoTracking().Where(predicate).OrderBy(g => g.ReleaseDate).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync() : await _context.Games.AsNoTracking().Where(predicate).OrderByDescending(g => g.ReleaseDate).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();
                    break;
                case Constants.ORDER_GAME_RATING:
                    games = sortOrder == Constants.ASC ? await _context.Games.AsNoTracking().Where(predicate).OrderBy(g => g.Playthroughs.Where(p => p.Rating > 0).Select(p => p.Rating).DefaultIfEmpty().Average()).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync()
                        : await _context.Games.AsNoTracking().Where(predicate).OrderByDescending(g => g.Playthroughs.Where(p => p.Rating > 0).Select(p => p.Rating).DefaultIfEmpty().Average()).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();
                    break;
                case Constants.ORDER_GAME_TITLE:
                    games = sortOrder == Constants.ASC ? await _context.Games.AsNoTracking().Where(predicate).OrderBy(g => g.Slug).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync() : await _context.Games.AsNoTracking().Where(predicate).OrderByDescending(g => g.Slug).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();
                    break;
                default:
                    games = sortOrder == Constants.ASC ? await _context.Games.AsNoTracking().Where(predicate).OrderBy(g => g.Id).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync() : await _context.Games.AsNoTracking().Where(predicate).OrderByDescending(g => g.Id).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();
                    break;
            }

            PaginatedList<GameSummaryDTO> paginatedList = new PaginatedList<GameSummaryDTO>
            {
                Items = games.Select(g => new GameSummaryDTO
                {
                    Id = g.Id,
                    Name = g.Name,
                    Slug = g.Slug,
                    ReleaseDate = g.ReleaseDate,
                    CoverUrl = g.CoverUrl
                }).ToList(),
                TotalItems = totalItems,
                PageNumber = page,
                PageLimit = itemsPerPage
            };

            return paginatedList;
        }

        public async Task<PaginatedList<GameSummaryDTO>> GetPaginatedList(IQueryable<GameData> query, ExpressionStarter<GameData> predicate, string orderBy, string sortOrder, int totalItems, double min, double max, int page = 1)
        {
            List<GameData> games;
            switch (orderBy)
            {
                case Constants.ORDER_RELEASE_DATE:
                    games = sortOrder == Constants.ASC ? await query.Where(predicate).OrderBy(g => g.ReleaseDate).Skip(page - 1 * Constants.PAGE_LIMIT_A).Take(Constants.PAGE_LIMIT_A).ToListAsync() : await query.Where(predicate).OrderByDescending(g => g.ReleaseDate).Skip(page - 1 * Constants.PAGE_LIMIT_A).Take(Constants.PAGE_LIMIT_A).ToListAsync();
                    break;
                case Constants.ORDER_GAME_RATING:
                    games = sortOrder == Constants.ASC ? await query.Where(predicate).OrderBy(g => g.Playthroughs.Where(p => p.Rating > 0).Select(p => p.Rating).DefaultIfEmpty().Average()).Skip((page - 1) * Constants.PAGE_LIMIT_A).Take(Constants.PAGE_LIMIT_A).ToListAsync()
                        : await query.Where(predicate).OrderByDescending(g => g.Playthroughs.Where(p => p.Rating > 0).Select(p => p.Rating).DefaultIfEmpty().Average()).Skip((page - 1) * Constants.PAGE_LIMIT_A).Take(Constants.PAGE_LIMIT_A).ToListAsync();
                    break;
                case Constants.ORDER_GAME_TITLE:
                    games = sortOrder == Constants.ASC ? await query.Where(predicate).OrderBy(g => g.Slug).Skip(page - 1 * Constants.PAGE_LIMIT_A).Take(Constants.PAGE_LIMIT_A).ToListAsync() : await query.Where(predicate).OrderByDescending(g => g.Slug).Skip(page - 1 * Constants.PAGE_LIMIT_A).Take(Constants.PAGE_LIMIT_A).ToListAsync();
                    break;
                default:
                    games = sortOrder == Constants.ASC ? await query.Where(predicate).OrderBy(g => g.Id).Skip(page - 1 * Constants.PAGE_LIMIT_A).Take(Constants.PAGE_LIMIT_A).ToListAsync() : await query.Where(predicate).OrderByDescending(g => g.Id).Skip(page - 1 * Constants.PAGE_LIMIT_A).Take(Constants.PAGE_LIMIT_A).ToListAsync();
                    break;
            }

            return new PaginatedList<GameSummaryDTO>
            {
                TotalItems = totalItems,
                Items = games.Select(g => new GameSummaryDTO
                {
                    Id = g.Id,
                    Name = g.Name,
                    Slug = g.Slug,
                    ReleaseDate = g.ReleaseDate,
                    CoverUrl = g.CoverUrl
                }).ToList(),
                PageNumber = page,
                PageLimit = Constants.PAGE_LIMIT_A
            };
        }

        public async Task<PaginatedList<GameSummaryDTO>> GetPaginatedList(ExpressionStarter<GameData> predicate, Guid userId, string orderBy, string sortOrder, int page = 1, int itemsPerPage = 60)
        {
            var totalItems = await _context.Games.AsNoTracking().CountAsync(predicate);

            List<GameData> games;
            List<GameData>? queried = null;
            switch (orderBy)
            {
                case Constants.ORDER_RELEASE_DATE:
                    games = sortOrder == Constants.ASC ? await _context.Games.AsNoTracking().Where(predicate).OrderBy(g => g.ReleaseDate).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync() : await _context.Games.AsNoTracking().Where(predicate).OrderByDescending(g => g.ReleaseDate).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();
                    break;
                case Constants.ORDER_GAME_RATING:
                    games = sortOrder == Constants.ASC ? await _context.Games.AsNoTracking().Where(predicate).OrderBy(g => g.Playthroughs.Where(p => p.Rating > 0).Select(p => p.Rating).DefaultIfEmpty().Average()).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync()
                        : await _context.Games.AsNoTracking().Where(predicate).OrderByDescending(g => g.Playthroughs.Where(p => p.Rating > 0).Select(p => p.Rating).DefaultIfEmpty().Average()).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();
                    break;
                case Constants.ORDER_GAME_TITLE:
                    games = sortOrder == Constants.ASC ? await _context.Games.AsNoTracking().Where(predicate).OrderBy(g => g.Slug).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync() : await _context.Games.AsNoTracking().Where(predicate).OrderByDescending(g => g.Slug).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();
                    break;
                case Constants.ORDER_LAST_PLAYED:
                    queried = await _context.Games.AsNoTracking().Where(predicate).Include(g => g.GameLogs).ThenInclude(l => l.Playthroughs).ToListAsync();
                    queried.Sort(new LastPlayedComparer());
                    if (sortOrder == Constants.DESC)
                        queried.Reverse();

                    games = queried.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case Constants.ORDER_WHEN_ADDED:
                    queried = await _context.Games.AsNoTracking().Where(predicate).Include(g => g.GameLogs).ThenInclude(l => l.Playthroughs).ToListAsync();
                    queried.Sort(new UserGameAddedComparer());
                    if (sortOrder == Constants.DESC)
                        queried.Reverse();

                    games = queried.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                case Constants.ORDER_USER_RATING:
                    queried = await _context.Games.AsNoTracking().Where(predicate).Include(g => g.GameLogs).ThenInclude(l => l.Playthroughs).ToListAsync();
                    queried.Sort(new UserRatingComparer());
                    if (sortOrder == Constants.DESC)
                        queried.Reverse();
                        
                    games = queried.Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                    break;
                default:
                    games = sortOrder == Constants.ASC ? await _context.Games.AsNoTracking().Where(predicate).OrderBy(g => g.Id).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync() : await _context.Games.AsNoTracking().Where(predicate).OrderByDescending(g => g.Id).Skip((page - 1) * itemsPerPage).Take(itemsPerPage).ToListAsync();
                    break;
            }

            PaginatedList<GameSummaryDTO> paginatedList = new PaginatedList<GameSummaryDTO>
            {
                Items = games.Select(g => new GameSummaryDTO
                {
                    Id = g.Id,
                    Name = g.Name,
                    Slug = g.Slug,
                    ReleaseDate = g.ReleaseDate,
                    CoverUrl = g.CoverUrl
                }).ToList(),
                TotalItems = totalItems,
                PageNumber = page,
                PageLimit = itemsPerPage
            };

            return paginatedList;
        }
    }
}
