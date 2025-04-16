using gremlin_eye.Server.Entity;

namespace gremlin_eye.Server.Services
{
    public interface ITokenService
    {
        public string GenerateToken(AppUser user);
    }
}
