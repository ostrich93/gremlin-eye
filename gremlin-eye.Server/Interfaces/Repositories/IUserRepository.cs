using gremlin_eye.Server.Entity;

namespace gremlin_eye.Server.Repositories
{
    public interface IUserRepository
    {
        AppUser? GetUserByName(string username);
        AppUser? GetUserById(Guid userId);
        Task<AppUser?> GetUserByNameAsync(string username);

        Task<AppUser?> GetUserWithTokensAsync(string username);
        Task<AppUser> CreateUserAsync(AppUser user);
        //Task<AppUser?> GetUser(Guid id);
        //Task<List<AppUser>> GetAllUsers();
        
    }
}
