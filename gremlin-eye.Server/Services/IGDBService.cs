using IGDB;
using IGDB.Models;

namespace gremlin_eye.Server.Services
{
    public class IGDBService : IIGDBService
    {
        private readonly IConfiguration _configuration;
        private readonly IGDBClient igdb;
        private readonly ProxyClient? igdbProxy;

        private readonly string _clientId;
        private readonly string _clientSecret;

        public IGDBService(IConfiguration configuration)
        {
            _configuration = configuration;
            _clientId = _configuration["Authentication:Igdb:ClientId"] ?? throw new InvalidOperationException("Igdb ClientId is required");
            _clientSecret = _configuration["Authentication:Igdb:ClientSecret"] ?? throw new InvalidOperationException("Igdb Client Secret is required");

            string? proxyUrl = _configuration["Igdb:ProxyUrl"];
            igdb = new IGDBClient(_clientId, _clientSecret);
            if (proxyUrl == null)
            {
                igdbProxy = null;
            }
            else
            {
                string _apiKey = _configuration["Igdb:APIKey"] ?? throw new InvalidOperationException("Igdb API Key for the proxy url is required.");
                igdbProxy = new ProxyClient(_clientId, _clientSecret, proxyUrl, _apiKey);
            }
        }

        public async Task<Game[]> GetGames(int offset)
        {
            if (igdbProxy != null)
                return await igdbProxy.QueryAsync<Game>(IGDBClient.Endpoints.Games,
                    $"fields checksum, collections.checksum, collections.id, collections.name, collections.slug, cover.image_id," +
                    $"first_release_date, genres.checksum, genres.id, genres.slug, genres.name, involved_companies.company.checksum, involved_companies.company.id, involved_companies.company.name, " +
                    $"involved_companies.company.slug, involved_companies.company.description, name, parent_game.id, platforms.checksum, platforms.id, platforms.name, " +
                    $"platforms.slug, summary, slug, screenshots.image_id; limit 500; offset {offset}; sort id asc;");
            else
                return await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games,
                    $"fields checksum, collections.checksum, collections.id, collections.name, collections.slug, cover.image_id," +
                    $"first_release_date, genres.checksum, genres.id, genres.slug, genres.name, involved_companies.company.checksum, involved_companies.company.id, involved_companies.company.name, " +
                    $"involved_companies.company.slug, involved_companies.company.description, name, parent_game.id, platforms.checksum, platforms.id, platforms.name, " +
                    $"platforms.slug, summary, slug, screenshots.image_id; limit 500; offset {offset}; sort id asc;");
        }

        public async Task<Game?> QueryGameAsync(string slug)
        {
            Game[] games;
            if (igdbProxy != null)
                games = await igdbProxy.QueryAsync<Game>(IGDBClient.Endpoints.Games,
                $"fields collections.id, collections.name, collections.slug, cover.id," +
                $"first_release_date, genres.id, genres.slug, genres.name, involved_companies.id, involved_companies.name, " +
                $"involved_companies.slug, involved_companies.description, parent_game.id, platforms.id, platforms.name, " +
                $"platforms.slug, summary, slug, screenshots.url; where slug = \"{slug}\";");
            else
                games = await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games,
                    $"fields collections.id, collections.name, collections.slug, cover.id," +
                    $"first_release_date, genres.id, genres.slug, genres.name, involved_companies.id, involved_companies.name, " +
                    $"involved_companies.slug, involved_companies.description, parent_game.id, platforms.id, platforms.name, " +
                    $"platforms.slug, summary, slug, screenshots.url; where slug = \"{slug}\";");
            
            return games != null ? games[0] : null;
        }

        public async Task<ICollection<Game>> QuickSearchGamesAsync(string searchItem)
        {
            if (igdbProxy != null)
                return await igdbProxy.QueryAsync<Game>(IGDBClient.Endpoints.Games,
                    $"""search "{searchItem}"; fields checksum, id, name, slug, first_release_date; limit 50;""");
            else
                return await igdb.QueryAsync<Game>(IGDBClient.Endpoints.Games,
                    $"""search "{searchItem}"; fields checksum, id, name, slug, first_release_date; limit 50;""");
        }

        public async Task<Genre[]> GetGenres()
        {
            if (igdbProxy != null)
            {
                return await igdbProxy.QueryAsync<Genre>(IGDBClient.Endpoints.Genres,
                    $"fields checksum, id, name, slug; limit 30; sort id asc;"
                );
            }
            else
                return await igdb.QueryAsync<Genre>(IGDBClient.Endpoints.Genres,
                    $"fields checksum, id, name, slug; limit 30; sort id asc;"
                );
        }

        public async Task<Collection[]> GetSeries(int offset)
        {
            if (igdbProxy != null)
                return await igdbProxy.QueryAsync<Collection>(IGDBClient.Endpoints.Collections,
                    $"fields checksum, id, name, slug; limit 500; offset {offset}; sort id asc;");
            else
                return await igdb.QueryAsync<Collection>(IGDBClient.Endpoints.Collections,
                    $"fields checksum, id, name, slug; limit 500; offset {offset}; sort id asc;"
                );
        }

        public async Task<Platform[]> GetPlatforms()
        {
            if (igdbProxy != null)
                return await igdbProxy.QueryAsync<Platform>(IGDBClient.Endpoints.Platforms,
                $"fields checksum, id, name, slug; limit 500; sort id asc;"
            );
            else
                return await igdb.QueryAsync<Platform>(IGDBClient.Endpoints.Platforms,
                    $"fields checksum, id, name, slug; limit 500; sort id asc;"
                );
        }

        public async Task<Company[]> GetCompanies(int offset)
        {
            if (igdbProxy != null)
                return await igdbProxy.QueryAsync<Company>(IGDBClient.Endpoints.Companies,
                $"fields checksum, id, description, name, slug; limit 500; offset {offset}; sort id asc;"
            );
            else
                return await igdb.QueryAsync<Company>(IGDBClient.Endpoints.Companies,
                    $"fields checksum, id, description, name, slug; limit 500; offset {offset}; sort id asc;"
                );
        }
    }
}
