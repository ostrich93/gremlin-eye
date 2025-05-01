using System.ComponentModel.DataAnnotations;

namespace gremlin_eye.Server.Entity
{
    public class RefreshToken
    {
        [Key]
        public long TokenId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresIn { get; set; }

        public Guid UserId { get; set; }
        public AppUser User { get; set; }

        public DateTime? Revoked { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresIn;
        public bool IsRevoked => Revoked != null;
        public bool IsActive => !IsExpired && !IsRevoked;
    }
}
