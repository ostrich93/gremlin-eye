using gremlin_eye.Server.Data;
using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Entity;

namespace gremlin_eye.Server.Services
{
    public class UserService : IUserService
    {
        private readonly IPasswordHasher _passwordHasher;
        private UnitOfWork _unitOfWork;

        public UserService(IPasswordHasher passwordHasher, UnitOfWork unitOfWork)
        {
            _passwordHasher = passwordHasher;
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

        
        public async Task<AppUser?> GetUserByName(string username)
        {
            return await _unitOfWork.Users.GetUserByNameAsync(username);
        }
    }
}
