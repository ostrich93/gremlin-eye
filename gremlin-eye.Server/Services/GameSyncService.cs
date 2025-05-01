using gremlin_eye.Server.Data;
using gremlin_eye.Server.Entity;
using IGDB;
using IGDB.Models;

namespace gremlin_eye.Server.Services
{
    public class GameSyncService : IGameSyncService
    {
        private UnitOfWork _unitOfWork;
        private readonly IIGDBService _igdbService;
        private readonly IGameService _gameService;

        private int LIMIT = 500;

        public GameSyncService(UnitOfWork unitOfWork, IIGDBService igdbService, IGameService gameService)
        {
            _unitOfWork = unitOfWork;
            _igdbService = igdbService;
            _gameService = gameService;
        }
        public async Task Import(int page = 1)
        {
            var games = await ImportGames(page);

            if (games.Length == 0)
                return;

            _unitOfWork.Context.Games.UpdateRange(games);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task<GameData[]> ImportGames(int page)
        {
            var offset = (page - 1) * LIMIT;

            var sourceGames = await _igdbService.GetGames(offset);
            GameData[] gameDatas = new GameData[LIMIT];
            int counter = 0;

            foreach(Game source in sourceGames)
            {
                if (source.Id == null)
                    continue;

                GameData? internalGame = await _gameService.GetGameById((long)source.Id);
                if (internalGame == null)
                {
                    internalGame = new GameData
                    {
                        GameId = (long)source.Id,
                        Slug = source.Slug,
                        Name = source.Name,
                        CoverUrl = ImageHelper.GetImageUrl(source.Cover.Value.ImageId, ImageSize.CoverBig),
                        BannerUrl = source.Screenshots.Values.Length > 0 ? ImageHelper.GetImageUrl(source.Screenshots.Values[0].ImageId, ImageSize.HD1080) : "",
                        Summary = source.Summary,
                        ReleaseDate = source.FirstReleaseDate
                    };
                }

                HandleGenres(internalGame, source);
                HandleCompanies(internalGame, source);
                HandlePlatforms(internalGame, source);
                HandleSeries(internalGame, source);

                gameDatas[counter] = internalGame;
                counter++;
            }
            return gameDatas;
        }

        private void HandleGenres(GameData gameData, Game sourceData)
        {
            var existingGenres = _unitOfWork.Context.Genres.ToList();

            foreach (var sourceGenre in sourceData.Genres.Values)
            {
                var genre = existingGenres.Where(g => g.GenreId == sourceGenre.Id).FirstOrDefault();
                if (genre == null)
                {
                    genre = new GenreData
                    {
                        GenreId = (long)sourceGenre.Id,
                        Name = sourceGenre.Name,
                        Slug = sourceGenre.Slug
                    };
                    _unitOfWork.Context.Genres.Add(genre);
                    _unitOfWork.Context.SaveChanges();
                }

                if (!gameData.Genres.Contains(genre))
                {
                    gameData.Genres.Add(genre);
                }
            }
        }

        private void HandleCompanies(GameData gameData, Game sourceData)
        {
            var existingCompanies = _unitOfWork.Context.Companies.ToList();
            var involvedCompanies = sourceData.InvolvedCompanies.Values.Select(ic => ic.Company.Value).ToList();

            foreach(var sourceCompany in involvedCompanies)
            {
                var company = existingCompanies.Where(c => c.CompanyId == sourceCompany.Id).FirstOrDefault();
                if (company == null)
                {
                    company = new CompanyData
                    {
                        CompanyId = (long)sourceCompany.Id,
                        Description = sourceCompany.Description,
                        Name = sourceCompany.Name,
                        Slug = sourceCompany.Slug
                    };
                    _unitOfWork.Context.Companies.Add(company);
                    _unitOfWork.Context.SaveChanges();
                }
                if (!gameData.Companies.Contains(company))
                {
                    gameData.Companies.Add(company);
                }
            }
        }

        private void HandlePlatforms(GameData gameData, Game sourceData)
        {
            var existingPlatforms = _unitOfWork.Context.Platforms.ToList();

            foreach (var sourcePlatform in sourceData.Platforms.Values)
            {
                var platform = existingPlatforms.Where(g => g.PlatformId == sourcePlatform.Id).FirstOrDefault();
                if (platform == null)
                {
                    platform = new PlatformData
                    {
                        PlatformId = (long)sourcePlatform.Id,
                        Name = sourcePlatform.Name,
                        Slug = sourcePlatform.Slug
                    };
                    _unitOfWork.Context.Platforms.Add(platform);
                    _unitOfWork.Context.SaveChanges();
                }

                if (!gameData.Platforms.Contains(platform))
                {
                    gameData.Platforms.Add(platform);
                }
            }
        }

        private void HandleSeries(GameData gameData, Game sourceData)
        {
            var existingSeries = _unitOfWork.Context.Series.ToList();

            foreach (var sourceSeries in sourceData.Collections.Values)
            {
                var series = existingSeries.Where(g => g.SeriesId == sourceSeries.Id).FirstOrDefault();
                if (series == null)
                {
                    series = new Series
                    {
                        SeriesId = (long)sourceSeries.Id,
                        Name = sourceSeries.Name,
                        Slug = sourceSeries.Slug
                    };

                    //Handle Child Series later
                    _unitOfWork.Context.Series.Add(series);
                    _unitOfWork.Context.SaveChanges();
                }

                if (!gameData.Series.Contains(series))
                {
                    gameData.Series.Add(series);
                }
            }
        }
    }
}
