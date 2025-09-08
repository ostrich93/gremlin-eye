using gremlin_eye.Server.Data;
using gremlin_eye.Server.Entity;
using IGDB;
using IGDB.Models;
using Microsoft.EntityFrameworkCore;

namespace gremlin_eye.Server.Services
{
    public class GameSyncService : IGameSyncService
    {
        //private const string imageUrlPrefix = "https:";

        private readonly IUnitOfWork _unitOfWork;
        private readonly IIGDBService _igdbService;

        private int LIMIT = 500;

        public GameSyncService(IUnitOfWork unitOfWork, IIGDBService igdbService)
        {
            _unitOfWork = unitOfWork;
            _igdbService = igdbService;
        }

        public async Task<long> ImportGenres()
        {
            var sourceGenres = await _igdbService.GetGenres();
            if (sourceGenres.Length == 0)
                return -1;

            var existingGenres = await _unitOfWork.Context.Genres.ToListAsync();
            List<GenreData> newGenres = new List<GenreData>();
            List<GenreData> updatedGenres = new List<GenreData>();
            foreach(Genre source in sourceGenres)
            {
                if (source.Id == null)
                    continue;
                GenreData? existingGenre = existingGenres!.FirstOrDefault(g => g.Id == source.Id);
                if (existingGenre != null)
                {
                    if (existingGenre.Checksum == source.Checksum) //no differences,so continue
                        continue;
                    else
                    {
                        existingGenre.Name = existingGenre.Name;
                        existingGenre.Slug = existingGenre.Slug;
                        existingGenre.Checksum = source.Checksum;
                        updatedGenres.Add(existingGenre);
                    }
                    updatedGenres.Add(existingGenre);
                }
                else
                {
                    newGenres.Add(new GenreData
                    {
                        Checksum = source.Checksum,
                        Id = (long)source.Id,
                        Name = source.Name,
                        Slug = source.Slug
                    });
                }
            }

            long lastChangedId = -1;

            if (newGenres.Count > 0)
            {
                _unitOfWork.Context.Genres.AddRange(newGenres);
                await _unitOfWork.SaveChangesAsync();
                lastChangedId = newGenres.Last().Id;
            }
            if (updatedGenres.Count > 0)
            {
                _unitOfWork.Context.Genres.UpdateRange(updatedGenres);
                await _unitOfWork.SaveChangesAsync();
                long lastId = updatedGenres.Last().Id;
                if (lastId > lastChangedId)
                    lastChangedId = lastId;
            }

            return lastChangedId;
        }

        public async Task<long> ImportPlatforms()
        {
            var sourcePlatforms = await _igdbService.GetPlatforms();
            if (sourcePlatforms.Length == 0)
                return -1;

            var existingPlatforms = await _unitOfWork.Context.Platforms.ToListAsync();
            List<PlatformData> newPlatforms = new List<PlatformData>();
            List<PlatformData> updatedPlatforms = new List<PlatformData>();
            foreach(Platform source in sourcePlatforms)
            {
                if (source.Id == null)
                    continue;
                PlatformData? existingPlatform = existingPlatforms!.FirstOrDefault(p => p.Id == source.Id);
                if (existingPlatform != null)
                {
                    if (existingPlatform.Checksum == source.Checksum)
                        continue;
                    else
                    {
                        existingPlatform.Checksum = source.Checksum;
                        existingPlatform.Name = source.Name;
                        existingPlatform.Slug = source.Slug;
                        updatedPlatforms.Add(existingPlatform);
                    }
                }
                else
                {
                    newPlatforms.Add(new PlatformData
                    {
                        Checksum = source.Checksum,
                        Id = (long)source.Id,
                        Name = source.Name,
                        Slug = source.Slug
                    });
                }
            }

            long lastChangedId = -1;

            if (newPlatforms.Count > 0)
            {
                _unitOfWork.Context.Platforms.AddRange(newPlatforms);
                await _unitOfWork.Context.SaveChangesAsync();
                lastChangedId = newPlatforms.Last().Id;
            }
            if (existingPlatforms.Count > 0)
            {
                _unitOfWork.Context.Platforms.UpdateRange(updatedPlatforms);
                await _unitOfWork.SaveChangesAsync();
                long lastId = updatedPlatforms.Last().Id;
                if (lastId > lastChangedId)
                    lastChangedId = lastId;
            }

            return lastChangedId;
        }

        public async Task<long> ImportSeries(int page = 1)
        {
            var offset = (page - 1) * LIMIT;

            var sourceSeries = await _igdbService.GetSeries(offset);
            HashSet<SeriesData> newSeries = new HashSet<SeriesData>();
            HashSet<SeriesData> updatedSeries = new HashSet<SeriesData>();

            foreach(Collection source in sourceSeries)
            {
                if (source.Id == null)
                    continue;

                SeriesData? internalSeries = await _unitOfWork.Context.Series.FirstOrDefaultAsync(s => s.Id == source.Id);
                if (internalSeries != null)
                {
                    if (internalSeries.Checksum == source.Checksum)
                        continue;
                    else
                    {
                        internalSeries.Checksum = source.Checksum;
                        internalSeries.Name = source.Name;
                        internalSeries.Slug = source.Slug;

                        updatedSeries.Add(internalSeries);
                    }
                }
                else
                {
                    internalSeries = new SeriesData
                    {
                        Checksum = source.Checksum,
                        Id = (long)source.Id,
                        Name = source.Name,
                        Slug = source.Slug
                    };
                    newSeries.Add(internalSeries);
                }
            }

            long lastChangedId = -1;

            if (newSeries.Count > 0)
            {
                _unitOfWork.Context.Series.AddRange(newSeries);
                await _unitOfWork.SaveChangesAsync();
                lastChangedId = newSeries.Last().Id;
            }
            if (updatedSeries.Count > 0)
            {
                _unitOfWork.Context.Series.UpdateRange(updatedSeries);
                await _unitOfWork.Context.SaveChangesAsync();
                long lastId = updatedSeries.Last().Id;
                if (lastId > lastChangedId)
                    lastChangedId = lastId;
            }

            return lastChangedId;
        }

        public async Task<long> ImportCompanies(int page = 1)
        {
            var offset = (page - 1) * LIMIT;

            var sourceCompanies = await _igdbService.GetCompanies(offset);
            HashSet<CompanyData> newCompanies = new HashSet<CompanyData>();
            HashSet<CompanyData> updatedCompanies = new HashSet<CompanyData>();

            foreach (Company source in sourceCompanies)
            {
                if (source.Id == null)
                    continue;

                CompanyData? internalCompany = await _unitOfWork.Context.Companies.FirstOrDefaultAsync(c => c.Id == source.Id);
                if (internalCompany != null)
                {
                    if (internalCompany.Checksum == source.Checksum)
                        continue;
                    else
                    {
                        internalCompany.Checksum = source.Checksum;
                        internalCompany.Name = source.Name;
                        internalCompany.Slug = source.Slug;
                        internalCompany.Description = source.Description;

                        updatedCompanies.Add(internalCompany);
                    }
                }
                else
                {
                    internalCompany = new CompanyData
                    {
                        Checksum = source.Checksum,
                        Id = (long)source.Id,
                        Name = source.Name,
                        Slug = source.Slug,
                        Description = source.Description
                    };

                    newCompanies.Add(internalCompany);
                }
            }

            long lastChangedId = -1;

            if (newCompanies.Count > 0)
            {
                _unitOfWork.Context.Companies.AddRange(newCompanies);
                await _unitOfWork.SaveChangesAsync();
                lastChangedId = newCompanies.Last().Id;
            }
            if (updatedCompanies.Count > 0) {
                _unitOfWork.Context.Companies.UpdateRange(updatedCompanies);
                await _unitOfWork.SaveChangesAsync();
                long lastId = updatedCompanies.Last().Id;
                if (lastId > lastChangedId)
                    lastChangedId = lastId;
            }

            return lastChangedId;
        }

        public async Task<long> ImportGames(int page = 1)
        {
            var offset = (page - 1) * LIMIT;

            var sourceGames = await _igdbService.GetGames(offset);
            
            //GameData[] gameDatas = new GameData[LIMIT];
            HashSet<GameData> gamesToUpdate = new HashSet<GameData>();
            HashSet<GameData> gamesToAdd = new HashSet<GameData>();

            HashSet<GenreData> existingGenres = _unitOfWork.Context.Genres.ToHashSet();
            HashSet<PlatformData> existingPlatforms = _unitOfWork.Context.Platforms.ToHashSet();
            HashSet<SeriesData> existingSeries = await _unitOfWork.Context.Series.ToHashSetAsync();
            HashSet<CompanyData> existingCompanies = await _unitOfWork.Context.Companies.ToHashSetAsync();

            long lastChangedId = -1;

            foreach (Game source in sourceGames)
            {
                if (source.Id == null)
                    continue;

                bool shouldCreate = false;
                int rowsToUpdate = 0;
                GameData? internalGame = await _unitOfWork.Games.GetGameById((long)source.Id);
                if (internalGame != null)
                {
                    if (internalGame.Checksum != source.Checksum)
                    {
                        internalGame.Checksum = source.Checksum;
                        internalGame.Name = source.Name;
                        internalGame.Slug = source.Slug;
                        internalGame.ReleaseDate = source.FirstReleaseDate;
                        internalGame.Summary = source.Summary ?? string.Empty;
                        internalGame.BannerUrl = source.Screenshots != null && source.Screenshots.Values.Length > 0 ? ImageHelper.GetImageUrl(source.Screenshots.Values[0].ImageId, ImageSize.HD1080) : string.Empty;
                        internalGame.CoverUrl = source.Cover != null && source.Cover.Value != null ? ImageHelper.GetImageUrl(source.Cover.Value.ImageId, ImageSize.CoverBig, true) : string.Empty;
                    }
                    _unitOfWork.Context.Games.Update(internalGame);
                    lastChangedId = internalGame.Id;
                }
                else if (internalGame == null)
                {
                    internalGame = new GameData
                    {
                        Id = (long)source.Id,
                        Slug = source.Slug,
                        Name = source.Name,
                        CoverUrl = source.Cover != null && source.Cover.Value != null ? ImageHelper.GetImageUrl(source.Cover.Value.ImageId, ImageSize.CoverBig): string.Empty,
                        BannerUrl = source.Screenshots != null && source.Screenshots.Values.Length > 0 ? ImageHelper.GetImageUrl(source.Screenshots.Values[0].ImageId, ImageSize.HD1080, true) : string.Empty,
                        Summary = source.Summary ?? string.Empty,
                        ReleaseDate = source.FirstReleaseDate,
                        Checksum = source.Checksum
                    };
                    //await _unitOfWork.Games.CreateAndSaveAsync(internalGame);
                    _unitOfWork.Context.Add(internalGame);
                    lastChangedId = internalGame.Id;
                }

                if (source.Genres != null && source.Genres.Values.Length > 0)
                    rowsToUpdate += HandleGenres(internalGame, source, existingGenres);
                if (source.Platforms != null && source.Platforms.Values.Length > 0)
                    rowsToUpdate += HandlePlatforms(internalGame, source, existingPlatforms);
                if (source.Collections != null && source.Collections.Values.Length > 0)
                    rowsToUpdate += HandleSeries(internalGame, source, existingSeries);
                if (source.InvolvedCompanies != null && source.InvolvedCompanies.Values.Length > 0)
                    rowsToUpdate += HandleCompanies(internalGame, source, existingCompanies);

            }

            await _unitOfWork.SaveChangesAsync();

            return lastChangedId;
        }

        private int HandleGenres(GameData gameData, Game sourceData, HashSet<GenreData> existingGenres)
        {
            var localGenres = gameData.GameGenres;

            HashSet<GameGenre> genresToDelete = new HashSet<GameGenre>();
            HashSet<GenreData> genresToUpdate = new HashSet<GenreData>();

            //if there are more genres for the game locally than in IGDB, remove the excess genres from the game
            if (localGenres.Count > sourceData.Genres.Values.Length)
            {
                IEnumerable<long> sourceIds = sourceData.Genres.Values.Where(g => g.Id != null).Select(g => (long)g.Id).ToArray();
                IEnumerable<GameGenre> localOnlyGenres = localGenres.ExceptBy(sourceIds, s => s.GenreId);
                if (localOnlyGenres is not null)
                {
                    foreach (GameGenre lGenre in localOnlyGenres)
                        genresToDelete.Add(lGenre);
                }
            }
            //if the game has more genres in IGDB than it does locally, handle the differences
            else if (localGenres.Count < sourceData.Genres.Values.Length)
            {
                var localGenreIds = localGenres.Select(l => l.GenreId).ToArray();
                IEnumerable<Genre> sourceOnlyGenres = sourceData.Genres.Values.ExceptBy(localGenreIds, g => (long)g.Id);
                if (sourceOnlyGenres is not null)
                {
                    foreach (var sourceGenre in sourceOnlyGenres)
                    {
                        //Check if the genre from IGDB even exists in the local database. If not, ignore it
                        //In both cases, add the genre to game's list
                        var existingGenre = existingGenres.FirstOrDefault(g => g.Id == sourceGenre.Id);
                        if (existingGenre != null) {
                            _unitOfWork.Context.GameGenres.Add(new GameGenre
                            {
                                GameId = gameData.Id,
                                GenreId = existingGenre.Id,
                                Game = gameData,
                                Genre = existingGenre
                            });
                        }
                    }
                }
            }

            //Check if any existing local genre data needs to be updated to match IGDB changes.
            foreach (var sourceGenre in sourceData.Genres.Values)
            {
                var genre = existingGenres.Where(g => g.Id == sourceGenre.Id).FirstOrDefault();
                if (genre != null && genre.Checksum != sourceGenre.Checksum)
                {
                    genre.Name = sourceGenre.Name;
                    genre.Slug = sourceGenre.Slug;
                    genre.Checksum = sourceGenre.Checksum;

                    genresToUpdate.Add(genre);
                }
            }

            if (genresToDelete.Count > 0)
                _unitOfWork.Context.GameGenres.RemoveRange(genresToDelete);
            if (genresToUpdate.Count > 0)
            {
                _unitOfWork.Context.Genres.UpdateRange(genresToUpdate);
                //_unitOfWork.Context.SaveChanges();
            }
            return genresToUpdate.Count + genresToDelete.Count;
        }

        private int HandleCompanies(GameData gameData, Game sourceData, HashSet<CompanyData> existingCompanies)
        {
            var involvedCompanies = sourceData.InvolvedCompanies.Values.Select(ic => ic.Company.Value).ToArray();
            var localCompanies = gameData.GameCompanies;

            HashSet<GameCompany> companiesToDelete = new HashSet<GameCompany>();
            HashSet<CompanyData> companiesToUpdate = new HashSet<CompanyData>();

            if (localCompanies.Count > involvedCompanies.Length)
            {
                var sourceCompanyIds = involvedCompanies.Select(c => c.Id).ToArray();
                IEnumerable<GameCompany> localOnlyCompanies = localCompanies.ExceptBy(sourceCompanyIds, c => c.CompanyId);
                if (localOnlyCompanies is not null)
                {
                    foreach (GameCompany lCompany in localOnlyCompanies)
                        companiesToDelete.Add(lCompany);
                }
            }
            else if (involvedCompanies.Length > localCompanies.Count)
            {
                var localCompanyIds = localCompanies.Select(c => c.CompanyId);
                IEnumerable<Company> sourceOnlyCompanies = involvedCompanies.ExceptBy(localCompanyIds, c => (long)c.Id);
                if (sourceOnlyCompanies is not null)
                {
                    foreach (var sourceCompany in sourceOnlyCompanies)
                    {
                        var existingCompany = existingCompanies.FirstOrDefault(c => c.Id == sourceCompany.Id);
                        if (existingCompany != null)
                            _unitOfWork.Context.GameCompanies.Add(new GameCompany
                            {
                                GameId = gameData.Id,
                                CompanyId = existingCompany.Id,
                                Company = existingCompany,
                                Game = gameData
                            });
                    }
                }
            }

            foreach (var sourceCompany in involvedCompanies)
            {
                long nonNullableSourceCompanyId = (long)sourceCompany.Id;
                var company = existingCompanies.Where(c => c.Id == nonNullableSourceCompanyId).FirstOrDefault();
                if (company != null && company.Checksum != sourceCompany.Checksum)
                {
                    company.Name = sourceCompany.Name;
                    company.Slug = sourceCompany.Slug;
                    company.Description = sourceCompany.Description;
                    company.Checksum = sourceCompany.Checksum;

                    companiesToUpdate.Add(company);
                }
            }

            if (companiesToDelete.Count > 0)
                _unitOfWork.Context.GameCompanies.RemoveRange(companiesToDelete);
            if (companiesToUpdate.Count > 0)
            {
                _unitOfWork.Context.Companies.UpdateRange(companiesToUpdate);
                //_unitOfWork.Context.SaveChanges();
            }

            return companiesToDelete.Count + companiesToUpdate.Count;
        }

        private int HandlePlatforms(GameData gameData, Game sourceData, HashSet<PlatformData> existingPlatforms)
        {
            var localPlatforms = gameData.GamePlatforms;

            HashSet<GamePlatform> platformsToDelete = new HashSet<GamePlatform>();
            HashSet<PlatformData> platformsToUpdate = new HashSet<PlatformData>();

            //remove excess platforms from the local data
            if (localPlatforms.Count > sourceData.Platforms.Values.Length)
            {
                IEnumerable<long> sourceIds = sourceData.Platforms.Values.Where(g => g.Id != null).Select(p => (long)p.Id).ToArray();
                IEnumerable<GamePlatform> localOnlyPlatforms = localPlatforms.ExceptBy(sourceIds, p => p.PlatformId);
                if (localOnlyPlatforms is not null)
                {
                    foreach (var localPlatform in localOnlyPlatforms)
                        platformsToDelete.Add(localPlatform);
                }
            }
            else if (localPlatforms.Count < sourceData.Platforms.Values.Length)
            {
                var localPlatformIds = localPlatforms.Select(l => l.PlatformId).ToArray();
                IEnumerable<Platform> sourceOnlyPlatforms = sourceData.Platforms.Values.ExceptBy(localPlatformIds, p => (long)p.Id);
                if (sourceOnlyPlatforms is not null)
                {
                    foreach (var sourcePlatform in sourceOnlyPlatforms)
                    {
                        var existingPlatform = existingPlatforms.FirstOrDefault(p => p.Id == sourcePlatform.Id);
                        if (existingPlatform != null)
                            _unitOfWork.Context.GamePlatforms.Add(new GamePlatform
                            {
                                GameId = gameData.Id,
                                PlatformId = existingPlatform.Id,
                                Game = gameData,
                                Platform = existingPlatform
                            });
                    }
                }
            }

            foreach (var sourcePlatform in sourceData.Platforms.Values)
            {
                var platform = existingPlatforms.Where(g => g.Id == sourcePlatform.Id).FirstOrDefault();
                if (platform != null && platform.Checksum != sourcePlatform.Checksum)
                {
                    platform.Name = sourcePlatform.Name;
                    platform.Slug = sourcePlatform.Slug;
                    platform.Checksum = sourcePlatform.Checksum;

                    platformsToUpdate.Add(platform);
                }
            }

            if (platformsToDelete.Count > 0)
                _unitOfWork.Context.GamePlatforms.RemoveRange(platformsToDelete);
            if (platformsToUpdate.Count > 0)
            {
                _unitOfWork.Context.Platforms.UpdateRange(platformsToUpdate);
                //_unitOfWork.Context.SaveChanges();
            }

            return platformsToDelete.Count + platformsToUpdate.Count;
        }

        private int HandleSeries(GameData gameData, Game sourceData, HashSet<SeriesData> existingSeries)
        {
            var localSeries = gameData.GameSeries;

            HashSet<GameSeries> seriesToDelete = new HashSet<GameSeries>();
            HashSet<SeriesData> seriesToUpdate = new HashSet<SeriesData>();

            if (localSeries.Count > sourceData.Collections.Values.Length)
            {
                IEnumerable<long> sourceIds = sourceData.Collections.Values.Where(c => c.Id != null).Select(c => (long)c.Id).ToArray();
                IEnumerable<GameSeries> localOnlySeries = localSeries.ExceptBy(sourceIds, s => s.SeriesId);
                if (localOnlySeries is not null)
                {
                    foreach (var localS in localOnlySeries)
                        seriesToDelete.Add(localS);
                }
            }
            else if (localSeries.Count < sourceData.Collections.Values.Length)
            {
                var localSeriesIds = localSeries.Select(s => s.SeriesId).ToArray();
                IEnumerable<Collection> sourceOnlySeries = sourceData.Collections.Values.ExceptBy(localSeriesIds, s => (long)s.Id);
                if (sourceOnlySeries is not null)
                {
                    foreach (var sourceSeries in sourceOnlySeries)
                    {
                        var existingS = existingSeries.FirstOrDefault(s => s.Id == sourceSeries.Id);
                        if (existingS != null)
                            _unitOfWork.Context.GameSeries.Add(new GameSeries
                            {
                                GameId = gameData.Id,
                                SeriesId = existingS.Id,
                                Game = gameData,
                                Series = existingS
                            });
                    }
                }
            }
                
            foreach (var sourceSeries in sourceData.Collections.Values)
            {
                var series = existingSeries.Where(g => g.Id == sourceSeries.Id).FirstOrDefault();
                if (series != null && series.Checksum != sourceSeries.Checksum)
                {
                    series.Name = sourceSeries.Name;
                    series.Slug = sourceSeries.Slug;
                    series.Checksum = sourceSeries.Checksum;

                    seriesToUpdate.Add(series);
                }
            }

            if (seriesToDelete.Count > 0)
                _unitOfWork.Context.GameSeries.RemoveRange(seriesToDelete);
            if (seriesToUpdate.Count > 0)
            {
                _unitOfWork.Context.Series.UpdateRange(seriesToUpdate);
                //_unitOfWork.Context.SaveChanges();
            }

            return seriesToDelete.Count + seriesToUpdate.Count;
        }
    }
}
