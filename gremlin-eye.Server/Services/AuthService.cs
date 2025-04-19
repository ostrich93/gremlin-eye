using gremlin_eye.Server.Data;
using gremlin_eye.Server.DTOs;

namespace gremlin_eye.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly UnitOfWork _unitOfWork;

        public AuthService(IPasswordHasher passwordHasher, ITokenService tokenService, UnitOfWork unitOfWork)
        {
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }
        public async Task<UserResponseDTO> LoginAsync(LoginDTO request)
        {
            var foundUser = await _unitOfWork.Users.GetUserByName(request.Username);

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

        public async Task LogoutAsync()
        {
            await Task.CompletedTask;
        }
    }
}
