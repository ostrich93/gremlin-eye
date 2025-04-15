using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Entity;
using Microsoft.AspNetCore.Identity;

namespace gremlin_eye.Server.Services
{
    public interface IUserService
    {
        Task<(IdentityResult, AppUser)> CreateUserAsync(RegisterUserRequestDTO request);
        Task<IdentityResult> AddRoleAsync(AppUser user, string role);
        Task<SignInResult> Login(LoginDTO request);
        Task<AppUser?> GetUserByName(string username);
        Task<IList<string>> GetRolesAsync(AppUser user);
    }
}
