using gremlin_eye.Server.Entity;
using System.Security.Claims;

namespace gremlin_eye.Server.Services
{
    public interface ITokenService
    {
        string GenerateAccessToken(AppUser user);
        string GenerateRefreshToken();
        string GeneratePasswordVerificationToken(AppUser user);
        ClaimsPrincipal? GetPrincipalFromToken(string token);
        bool ValidatePasswordVerificationToken(string code, AppUser user);
        bool CheckResetTokenExpiration(string token);
    }
}
