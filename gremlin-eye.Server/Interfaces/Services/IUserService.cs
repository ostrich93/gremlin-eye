using gremlin_eye.Server.DTOs;
using gremlin_eye.Server.Entity;

namespace gremlin_eye.Server.Services
{
    public interface IUserService
    {
        Task<UserResponseDTO> CreateUserAsync(RegisterUserRequestDTO request);
        Task<AppUser?> GetUserByName(string username);
        Task<UserProfileResponse> GetUserProile(string username);
    }
}
