using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Entity;

namespace gremlin_eye.Server.Services
{
    public interface IGameService
    {
        Task<GameData[]> GetGameData(int offset, int limit);
        Task<GameDetailsResponseDTO> GetGameDetailsBySlug(string slug, Guid? userId = null);
        Task<GameData?> GetGameById(long id);
    }
}
