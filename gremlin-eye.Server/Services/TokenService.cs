using gremlin_eye.Server.Entity;
using gremlin_eye.Server.Extensions;
using gremlin_eye.Server.Utilities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace gremlin_eye.Server.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _signingKey;

        public TokenService(IConfiguration config)
        {
            _config = config;
            _signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SigningKey"] ?? throw new InvalidOperationException("JWT Signing Key is not configured in the application settings")));
        }

        public string GenerateAccessToken(AppUser user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, user.UserName ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString() ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            SigningCredentials signingCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials = signingCredentials,
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                NotBefore = DateTime.UtcNow
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GeneratePasswordVerificationToken(AppUser user)
        {
            var ms = new MemoryStream();
            using (var writer = new BinaryWriter(ms, new UTF8Encoding(false, true), true))
            {
                writer.Write(DateTimeOffset.UtcNow);
                writer.Write(user.Id.ToString());
                writer.Write(user.Email);
                writer.Write(user.Stamp != null ? user.Stamp : "");
            }
            var protectedBytes = Protect(ms.ToArray(), DateTimeOffset.UtcNow.AddHours(2));
            return Convert.ToBase64String(protectedBytes);
        }

        private byte[] Protect(byte[] plaintext, DateTimeOffset expiration)
        {
            byte[] plaintextWithHeader = new byte[checked(8 + plaintext.Length)];
            StreamUtils.WriteUInt64(plaintextWithHeader, 0, (ulong)expiration.UtcTicks);
            Buffer.BlockCopy(plaintext, 0, plaintextWithHeader, 8, plaintext.Length);

            return plaintextWithHeader;
        }

        public bool ValidatePasswordVerificationToken(string code, AppUser user)
        {
            var plaintextWithHeader = Convert.FromBase64String(code);
            ulong utcTicksExpiration = StreamUtils.ReadUInt64(plaintextWithHeader, 0);
            DateTimeOffset embeddedExpiration = new DateTimeOffset(checked((long)utcTicksExpiration), TimeSpan.Zero /* UTC */); //extracts expiration date

            if (DateTimeOffset.UtcNow > embeddedExpiration)
            {
                return false;
            }

            string userId;
            string email;
            string stamp;

            byte[] retValue = new byte[plaintextWithHeader.Length - 8];
            Buffer.BlockCopy(plaintextWithHeader, 8, retValue, 0, retValue.Length);

            var ms = new MemoryStream(retValue);
            using (var reader = ms.CreateReader())
            {
                ms.Seek(8, SeekOrigin.Begin);

                userId = reader.ReadString();
                email = reader.ReadString();
                stamp = reader.ReadString();
            }

            if (!Guid.ParseExact(userId, "D").Equals(user.Id))
                return false;
            if (!email.Equals(user.Email))
                return false;

            if (string.IsNullOrWhiteSpace(user.Stamp) && !string.IsNullOrEmpty(stamp))
                return false;
            else if (!stamp.Equals(user.Stamp))
                return false;

            return true;
        }

        public string GenerateRefreshToken()
        {
            byte[] bytes = RandomNumberGenerator.GetBytes(32);
            return Convert.ToBase64String(bytes);
        }

        public ClaimsPrincipal? GetPrincipalFromToken(string token)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SigningKey"])),
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero
                };
                SecurityToken securityToken;
                return new JwtSecurityTokenHandler().ValidateToken(token, tokenValidationParameters, out securityToken);
            } catch
            {
                return null;
            }
        }

        public bool CheckResetTokenExpiration(string token)
        {
            var plaintextWithHeader = Convert.FromBase64String(token);
            ulong utcTicksExpiration = StreamUtils.ReadUInt64(plaintextWithHeader, 0);
            DateTimeOffset embeddedExpiration = new DateTimeOffset(checked((long)utcTicksExpiration), TimeSpan.Zero /* UTC */); //extracts expiration date

            return DateTimeOffset.UtcNow > embeddedExpiration;
        }
    }
}
