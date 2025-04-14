using gremlin_eye.Server.Entity;

namespace gremlin_eye.Server.Repositories
{
    public interface IUserRepository
    {
        public Task<AppUser?> GetUserByName(string username);
        //public Task<AppUser?> GetUser(Guid id);
        //public Task<List<AppUser>> GetAllUsers();
        
    }
}
