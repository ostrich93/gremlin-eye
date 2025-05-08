using IGDB;
using IGDB.Models;

namespace gremlin_eye.Server.Services
{
    public class IGDBService : IIGDBService
    {
        private readonly IConfiguration _configuration;
        private readonly IGDBClient igdb;

        private readonly string _clientId;
        private readonly string _clientSecret;

        public IGDBService(IConfiguration configuration)
        {
            _configuration = configuration;
            _clientId = _configuration["Authentication:Igdb:ClientId"] ?? throw new InvalidOperationException("Igdb ClientId is required");
            _clientSecret = _configuration["Authentication:Igdb:ClientSecret"] ?? throw new InvalidOperationException("Igdb Client Secret is required");
            igdb = new IGDBClient(_clientId, _clientSecret);
        }

        public async Task<Game[]> GetGames(int offset)
        {
            return await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games,
                $"fields checksum, collections.checksum, collections.id, collections.name, collections.slug, cover.id," +
                $"first_release_date, genres.checksum, genres.id, genres.slug, genres.name, involved_companies.company.checksum, involved_companies.company.id, involved_companies.company.name, " +
                $"involved_companies.company.slug, involved_companies.company.description, name, parent_game.id, platforms.checksum, platforms.id, platforms.name, " +
                $"platforms.slug, summary, slug, screenshots.url; limit 500; offset {offset}; sort id asc;");
        }

        public async Task<Game?> QueryGameAsync(string slug)
        {
            var games = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games,
                $"fields collections.id, collections.name, collections.slug, cover.id," +
                $"first_release_date, genres.id, genres.slug, genres.name, involved_companies.id, involved_companies.name, " +
                $"involved_companies.slug, involved_companies.description, parent_game.id, platforms.id, platforms.name, " +
                $"platforms.slug, summary, slug, screenshots.url; where slug = \"{slug}\";");
            return games != null ? games[0] : null;
        }

        public async Task<ICollection<Game>> QuickSearchGamesAsync(string searchItem)
        {
            return await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Search,
                $"""search "{searchItem}"; fields checksum, id, name, slug, first_release_date; limit 50;""");
        }

        public async Task<Genre[]> GetGenres()
        {
            return await igdb.QueryAsync<Genre>(IGDBClient.Endpoints.Genres,
                $"fields checksum, id, name, slug; limit 30; sort id asc;"
            );
        }

        public async Task<Collection[]> GetSeries(int offset)
        {
            return await igdb.QueryAsync<Collection>(IGDBClient.Endpoints.Collections,
                $"fields checksum, id, name, slug; limit 500; offset {offset}; sort id asc;"
            );
        }

        public async Task<Platform[]> GetPlatforms()
        {
            return await igdb.QueryAsync<Platform>(IGDBClient.Endpoints.Platforms,
                $"fields checksum, id, name, slug; limit 500; sort id asc;"
            );
        }

        public async Task<Company[]> GetCompanies(int offset)
        {
            return await igdb.QueryAsync<Company>(IGDBClient.Endpoints.Companies,
                $"fields checksum, id, description, name, slug; limit 500; offset {offset}; sort id asc;"
            );
        }
    }
}
