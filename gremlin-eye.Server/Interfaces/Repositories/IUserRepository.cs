using gremlin_eye.Server.Models;

namespace gremlin_eye.Server.Repositories
{
    public interface IUserRepository
    {
        public Task<User?> GetUser(int id);
        public Task<List<User>> GetAllUsers();
        //public Task<User> CreateUser(User user);
        //public Task<User> UpdateUser(User user);
        //public Task DeleteUser(User user);
    }
}
