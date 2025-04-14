using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Entity;
using gremlin_eye.Server.Repositories;
using Microsoft.AspNetCore.Identity;

namespace gremlin_eye.Server.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IUserRepository _userRepository;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, UserRepository userRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
        }

        public async Task<(IdentityResult, AppUser)> CreateUserAsync(RegisterUserRequestDTO request)
        {
            var user = new AppUser
            {
                UserName = request.Username,
                Email = request.Email,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            return (result, user);
        }

        public async Task<IdentityResult> AddRoleAsync(AppUser user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<SignInResult> Login(LoginDTO request)
        {
            return await _signInManager.PasswordSignInAsync(request.Username, request.Password, true, false);
        }

        public async Task<AppUser?> GetUserByName(string username)
        {
            return await _userRepository.GetUserByName(username);
        }

        public async Task<IList<string>> GetRolesAsync(AppUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }
    }
}
