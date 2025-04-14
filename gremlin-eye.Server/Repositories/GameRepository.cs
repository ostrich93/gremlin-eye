using gremlin_eye.Server.Data;
using gremlin_eye.Server.Entity;
using gremlin_eye.Server.Services;
using IGDB;
using IGDB.Models;
using Microsoft.EntityFrameworkCore;

namespace gremlin_eye.Server.Repositories
{
    public class GameRepository : IGameRepository
    {
        private readonly DataContext _context;
        private readonly IIGDBService _igdb;

        public GameRepository(DataContext context, IIGDBService igdbService)
        {
            _context = context;
            _igdb = igdbService;
        }

        public async Task<GameData?> GetGameBySlug(string slug)
        {
            var game = await _context.Games.Include(g => g.Companies).Include(g => g.Genres).Include(g => g.Series).Include(g => g.Platforms).Where(g => g.Slug == slug).FirstOrDefaultAsync();
            if (game == null)
            {
                var igdbGame = await _igdb.QueryGameAsync(slug);

                if (igdbGame == null)
                {
                    return null; //maybe throw an exception saying that the game doesn't exist in IGDB or our database?
                }

                var apiCompanies = igdbGame.InvolvedCompanies.Values.Select(ic => ic.Company.Value).ToList();
                var apiGenres = igdbGame.Genres.Values.ToList();
                var apiCollections = igdbGame.Collections.Values.ToList();
                var apiPlatforms = igdbGame.Platforms.Values.ToList();

                List<CompanyData> companies = new List<CompanyData>();
                List<GenreData> genres = new List<GenreData>();
                List<Series> series = new List<Series>();
                List<PlatformData> platforms = new List<PlatformData>();

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (apiCompanies.Count != 0)
                        {
                            foreach (Company company in apiCompanies)
                            {
                                var existingCompany = await _context.Companies.FirstOrDefaultAsync(c => c.CompanyId == company.Id);
                                if (existingCompany == null)
                                {
                                    CompanyData companyData = new CompanyData
                                    {
                                        CompanyId = (long)company.Id,
                                        Description = company.Description,
                                        Name = company.Name,
                                        Slug = company.Slug
                                    };
                                    await _context.Companies.AddAsync(companyData);
                                    await _context.SaveChangesAsync();
                                    companies.Add(companyData);
                                }
                            }
                        }
                        await transaction.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        await transaction.RollbackAsync();
                        return null;
                    }
                }

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (apiGenres.Count != 0)
                        {
                            foreach(Genre genre in apiGenres)
                            {
                                var existingGenre = await _context.Genres.FirstOrDefaultAsync(g => g.GenreId == genre.Id);
                                if (existingGenre == null)
                                {
                                    GenreData genreData = new GenreData
                                    {
                                        GenreId = (long)genre.Id,
                                        Name = genre.Name,
                                        Slug = genre.Slug
                                    };
                                    await _context.Genres.AddAsync(genreData);
                                    await _context.SaveChangesAsync();
                                    genres.Add(genreData);
                                }
                            }
                        }
                        await transaction.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        await transaction.RollbackAsync();
                        return null;
                    }
                }

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (apiPlatforms.Count != 0)
                        {
                            foreach (Platform platform in apiPlatforms)
                            {
                                var existingPlatform = await _context.Platforms.FirstOrDefaultAsync(p => p.PlatformId == platform.Id);
                                if (existingPlatform == null)
                                {
                                    PlatformData platformData = new PlatformData
                                    {
                                        PlatformId = (long)platform.Id,
                                        Name = platform.Name,
                                        Slug = platform.Slug
                                    };
                                    await _context.Platforms.AddAsync(platformData);
                                    await _context.SaveChangesAsync();
                                    platforms.Add(platformData);
                                }
                            }
                        }
                        await transaction.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        await transaction.RollbackAsync();
                        return null;
                    }
                }

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (apiCollections.Count != 0)
                        {
                            foreach (Collection collection in apiCollections)
                            {
                                var existingSeries = await _context.Series.FirstOrDefaultAsync(s => s.SeriesId == collection.Id);
                                if (existingSeries == null)
                                {
                                    Series seriesData = new Series
                                    {
                                        SeriesId = (long)collection.Id,
                                        Name = collection.Name,
                                        Slug = collection.Slug
                                    };
                                    await _context.Series.AddAsync(seriesData);
                                    await _context.SaveChangesAsync();
                                    series.Add(seriesData);
                                }
                            }
                        }
                        await transaction.CommitAsync();
                    }
                    catch (Exception e)
                    {
                        await transaction.RollbackAsync();
                        return null;
                    }
                }

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        game = new GameData
                        {
                            GameId = (long)igdbGame.Id,
                            Name = igdbGame.Name,
                            Slug = igdbGame.Slug,
                            CoverUrl = ImageHelper.GetImageUrl(imageId: igdbGame.Cover.Value.ImageId, size: ImageSize.CoverBig),
                            BannerUrl = igdbGame.Screenshots.Values.Length > 0 ? ImageHelper.GetImageUrl(imageId: igdbGame.Screenshots.Values[0].ImageId, ImageSize.HD1080) : "",
                            Summary = igdbGame.Summary,
                            ReleaseDate = igdbGame.FirstReleaseDate,
                            Companies = companies,
                            Series = series,
                            Platforms = platforms,
                            Genres = genres
                        };
                        _context.Games.Add(game);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                    } catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                    }
                }
            }
            return game;
        }
    }
}
