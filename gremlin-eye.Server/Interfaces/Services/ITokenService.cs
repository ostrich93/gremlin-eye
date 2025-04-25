using gremlin_eye.Server.Entity;
using System.Security.Claims;

namespace gremlin_eye.Server.Services
{
    public interface ITokenService
    {
        public string GenerateAccessToken(AppUser user);
        public string GenerateRefreshToken();
        public ClaimsPrincipal? GetPrincipalFromToken(string token);
    }
}
