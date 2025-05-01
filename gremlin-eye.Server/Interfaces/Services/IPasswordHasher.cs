namespace gremlin_eye.Server.Services
{
    public interface IPasswordHasher
    {
        void HashPassword(string password, out string hashedPassword, out byte[] salt);
        bool VerifyPassword(string inputPassword, string storedPassword, byte[] salt);
    }
}
