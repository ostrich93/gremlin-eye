using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace gremlin_eye.Server.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public void HashPassword(string password, out string hashedPassword, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(16);
            hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 1000,
                numBytesRequested: 32
                ));
        }

        public bool VerifyPassword(string inputPassword, string storedPassword, byte[] salt)
        {
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: inputPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 1000,
                numBytesRequested: 32
                ));
            return storedPassword == hashedPassword;
        }
    }
}
