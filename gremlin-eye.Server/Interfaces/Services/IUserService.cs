using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Entity;
using Microsoft.AspNetCore.Identity;

namespace gremlin_eye.Server.Services
{
    public interface IUserService
    {
        public Task<(IdentityResult, AppUser)> CreateUserAsync(RegisterUserRequestDTO request);
        public Task<IdentityResult> AddRoleAsync(AppUser user, string role);
        public Task<SignInResult> Login(LoginDTO request);
        public Task<AppUser?> GetUserByName(string username);
        public Task<IList<string>> GetRolesAsync(AppUser user);
    }
}
