using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Repositories;
using gremlin_eye.Server.Entity;
using IGDB.Models;

namespace gremlin_eye.Server.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IReviewRepository _reviewRepository;

        public GameService(IGameRepository gameRepository, IReviewRepository reviewRepository)
        {
            _gameRepository = gameRepository;
            _reviewRepository = reviewRepository;
        }

        public async Task<GameDetailsResponseDTO> GetGameDetailsBySlug(string slug, Guid? userId = null)
        {
            GameData? data = await _gameRepository.GetGameBySlug(slug);
            if (data != null)
            {
                int reviewCount = await _reviewRepository.GetGameReviewCount(data.GameId);
                return new GameDetailsResponseDTO
                {
                    Id = data.GameId,
                    Name = data.Name,
                    Slug = data.Slug,
                    CoverUrl = data.CoverUrl,
                    BannerUrl = data.BannerUrl,
                    Summary = data.Summary,
                    Date = data.ReleaseDate,
                    Platforms = data.Platforms.Select(p => new PlatformDTO
                    {
                        Id = p.PlatformId,
                        Name = p.Name,
                        Slug = p.Slug
                    }).ToList(),
                    Genres = data.Genres.Select(g => new GenreDTO
                    {
                        Id = g.GenreId,
                        Name = g.Name,
                        Slug = g.Slug
                    }).ToList(),
                    Companies = data.Companies.Select(c => new CompanyDTO
                    {
                        Id = c.CompanyId,
                        Name = c.Name,
                        Slug = c.Slug
                    }).ToList(),
                    Series = data.Series.Count > 0 ? new SeriesDTO
                    {
                        Id = data.Series.First().SeriesId,
                        Name = data.Series.First().Name,
                        Slug = data.Series.First().Slug
                    } : null,
                    ReviewCount = reviewCount
                };
            }
            throw new Exception("The game requested does not exist in our database or IGDB's.");
        }
    }
}
