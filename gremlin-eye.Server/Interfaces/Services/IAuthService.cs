﻿using gremlin_eye.Server.DTOs;

namespace gremlin_eye.Server.Services
{
    public interface IAuthService
    {
        Task<UserResponseDTO> LoginAsync(LoginDTO request);
        Task LogoutAsync(Guid userId, string refreshToken);
        TokenDTO RefreshToken(TokenDTO request);
    }
}
