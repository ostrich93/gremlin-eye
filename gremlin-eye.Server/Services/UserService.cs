using gremlin_eye.Server.Data;
using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Entity;

namespace gremlin_eye.Server.Services
{
    public class UserService : IUserService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private UnitOfWork _unitOfWork;

        public UserService(IPasswordHasher passwordHasher, ITokenService tokenService, UnitOfWork unitOfWork)
        {
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserResponseDTO> CreateUserAsync(RegisterUserRequestDTO request)
        {
            _passwordHasher.HashPassword(request.Password, out string hashedPassword, out byte[] salt);
            var user = new AppUser
            {
                UserName = request.Username,
                Email = request.Email,
                CreatedAt = DateTime.UtcNow,
                Role = request.Role,
                PasswordHash = hashedPassword,
                Salt = salt
            };
            var createdUser = await _unitOfWork.Users.CreateUserAsync(user);
            
            return new UserResponseDTO
            {
                UserId = createdUser.Id,
                Username = createdUser.UserName,
                Email = createdUser.Email,
                Role = createdUser.Role
            };
        }

        public async Task<UserResponseDTO> LoginAsync(LoginDTO request)
        {
            var foundUser = await GetUserByName(request.Username);

            if (foundUser == null)
            {
                throw new Exception($"User {request.Username} was not found");
            }

            bool verified = _passwordHasher.VerifyPassword(request.Password, foundUser.PasswordHash, foundUser.Salt);
            if (verified)
            {
                return new UserResponseDTO
                {
                    UserId = foundUser.Id,
                    Username = foundUser.UserName,
                    Email = foundUser.Email,
                    Role = foundUser.Role,
                    Token = _tokenService.GenerateToken(foundUser)
                };
            }
            else
            {
                throw new Exception("The password was invalid");
            }
        }

        public async Task<AppUser?> GetUserByName(string username)
        {
            return await _unitOfWork.Users.GetUserByName(username);
        }

        public async Task LogoutAsync()
        {
            await Task.CompletedTask;
        }
    }
}
