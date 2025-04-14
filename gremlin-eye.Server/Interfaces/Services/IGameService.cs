using gremlin_eye.Server.DTOs;

namespace gremlin_eye.Server.Services
{
    public interface IGameService
    {
        public Task<GameDetailsResponseDTO> GetGameDetailsBySlug(string slug, Guid? userId = null);
    }
}
