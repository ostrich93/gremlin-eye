using gremlin_eye.Server.Data;
using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Entity;
using System.Security.Claims;

namespace gremlin_eye.Server.Services
{
    public class AuthService : IAuthService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IPasswordHasher passwordHasher, ITokenService tokenService, IUnitOfWork unitOfWork)
        {
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }
        public async Task<UserResponseDTO> LoginAsync(LoginDTO request)
        {
            var foundUser = await _unitOfWork.Users.GetUserByNameAsync(request.Username);

            if (foundUser == null)
            {
                throw new Exception($"User {request.Username} was not found");
            }

            bool verified = _passwordHasher.VerifyPassword(request.Password, foundUser.PasswordHash, foundUser.Salt);
            if (verified)
            {
                var token = _tokenService.GenerateAccessToken(foundUser);
                var refreshToken = _tokenService.GenerateRefreshToken();
                foundUser.RefreshTokens.RemoveAll(t => !t.IsActive); //remove expired tokens
                foundUser.RefreshTokens.Add(new RefreshToken
                {
                    User = foundUser,
                    UserId = foundUser.Id,
                    Token = refreshToken,
                    ExpiresIn = DateTime.UtcNow.AddDays(7)
                });
                await _unitOfWork.Context.SaveChangesAsync();
                return new UserResponseDTO
                {
                    UserId = foundUser.Id,
                    Username = foundUser.UserName,
                    Email = foundUser.Email,
                    Role = foundUser.Role,
                    AccessToken = token,
                    RefreshToken = refreshToken
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

        public TokenDTO RefreshToken(TokenDTO request)
        {
            var accessToken = request.AccessToken;
            var refreshToken = request.RefreshToken;

            var principal = _tokenService.GetPrincipalFromToken(accessToken);
            var userId = Guid.Parse(principal.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

            var user = _unitOfWork.Users.GetUserById(userId);

            if (user == null)
            {
                throw new Exception("Invalid Token Request: User not found");
            }

            var userToken = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);
            if (userToken == null || !userToken.IsActive)
            {
                throw new Exception("Invalid Token Request: No active refresh token found");
            }

            //if (user == null || user.RefreshToken ==

            var newAccessToken = _tokenService.GenerateAccessToken(user);

            return new TokenDTO { AccessToken = newAccessToken, RefreshToken = refreshToken };

        }
    }
}
