using gremlin_eye.Server.Data;
using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Entity;
using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace gremlin_eye.Server.Controllers
{
    [Route("api/company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/:slug?query
        [HttpGet("{slug}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCompanyDetails (string slug,
            [FromQuery] string? releaseYear, [FromQuery] string? genre, [FromQuery] string? category, [FromQuery] string? platform,
            [FromQuery] double min = 0, [FromQuery] double max = 10,
            [FromQuery] string orderBy = Constants.ORDER_TRENDING, [FromQuery] string sortOrder = Constants.DESC,
            [FromQuery] int page = 1)
        {
            var company = _unitOfWork.Context.Companies.AsNoTracking().Where(c => c.Slug == slug).FirstOrDefault();
            if (company == null)
                return NotFound();

            var query = company.Games.AsQueryable();

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

            predicate.And(g => g.Playthroughs.Any(p => p.Rating >= min && p.Rating <= max));

            List<GameData> games;
            int totalItems = company.Games.Count(predicate);
            switch (orderBy)
            {
                case Constants.ORDER_RELEASE_DATE:
                    games = sortOrder == Constants.ASC ? await query.Where(predicate).OrderBy(g => g.ReleaseDate).Skip(page - 1 * Constants.PAGE_LIMIT_A).Take(Constants.PAGE_LIMIT_A).ToListAsync() : await query.Where(predicate).OrderByDescending(g => g.ReleaseDate).Skip(page - 1 * Constants.PAGE_LIMIT_A).Take(Constants.PAGE_LIMIT_A).ToListAsync();
                    break;
                case Constants.ORDER_GAME_RATING:
                    games = sortOrder == Constants.ASC ? await query.Where(predicate).OrderBy(g => g.Playthroughs.Where(p => p.Rating > 0).Select(p => p.Rating).DefaultIfEmpty().Average()).Skip(page - 1 * Constants.PAGE_LIMIT_A).Take(Constants.PAGE_LIMIT_A).ToListAsync()
                        : await query.Where(predicate).OrderByDescending(g => g.Playthroughs.Where(p => p.Rating > 0).Select(p => p.Rating).DefaultIfEmpty().Average()).Skip(page - 1 * Constants.PAGE_LIMIT_A).Take(Constants.PAGE_LIMIT_A).ToListAsync();
                    break;
                case Constants.ORDER_GAME_TITLE:
                    games = sortOrder == Constants.ASC ? await query.Where(predicate).OrderBy(g => g.Slug).Skip(page - 1 * Constants.PAGE_LIMIT_A).Take(Constants.PAGE_LIMIT_A).ToListAsync() : await query.Where(predicate).OrderByDescending(g => g.Slug).Skip(page - 1 * Constants.PAGE_LIMIT_A).Take(Constants.PAGE_LIMIT_A).ToListAsync();
                    break;
                default:
                    games = sortOrder == Constants.ASC ? await query.Where(predicate).OrderBy(g => g.Id).Skip(page - 1 * Constants.PAGE_LIMIT_A).Take(Constants.PAGE_LIMIT_A).ToListAsync() : await query.Where(predicate).OrderByDescending(g => g.Id).Skip(page - 1 * Constants.PAGE_LIMIT_A).Take(Constants.PAGE_LIMIT_A).ToListAsync();
                    break;
            }

            PaginatedList<GameSummaryDTO> paginatedList = new PaginatedList<GameSummaryDTO>
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

            return Ok(new CompanyDTO
            {
                Id = company.Id,
                Name = company.Name,
                Slug = company.Slug,
                Description = company.Description ?? "",
                Published = paginatedList
            });
        }
    }
}
