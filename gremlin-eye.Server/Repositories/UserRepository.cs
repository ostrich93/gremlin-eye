using gremlin_eye.Server.Data;
using gremlin_eye.Server.Entity;
using Microsoft.EntityFrameworkCore;

namespace gremlin_eye.Server.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<AppUser> CreateUserAsync(AppUser user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public AppUser? GetUserByName(string username)
        {
            return _context.Users.Include(u => u.RefreshTokens).FirstOrDefault(u => u.UserName == username);
        }

        public async Task<AppUser?> GetUserByNameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public AppUser? GetUserById(Guid userId)
        {
            return _context.Users.Include(u => u.RefreshTokens).FirstOrDefault(u => u.Id == userId);
            
        }

        public Task<AppUser?> GetUserWithTokensAsync(string username)
        {
            return _context.Users.Include(u => u.RefreshTokens).FirstOrDefaultAsync(u => u.UserName == username);
        }
    }
}
