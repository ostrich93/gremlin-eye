using gremlin_eye.Server.Entity;

namespace gremlin_eye.Server.Repositories
{
    public interface IUserRepository
    {
        Task<AppUser?> GetUserByName(string username);
        //Task<AppUser?> GetUser(Guid id);
        //Task<List<AppUser>> GetAllUsers();
        
    }
}
