using gremlin_eye.Server.Entity;

namespace gremlin_eye.Server.Services
{
    public interface IGameSyncService
    {
        Task Import(int page = 1);
        
    }
}
