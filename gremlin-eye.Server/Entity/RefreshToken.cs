using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gremlin_eye.Server.Entity
{
    public class RefreshToken
    {
        [Key]
        [Column("token_id")]
        public long TokenId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresIn { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }
        public AppUser User { get; set; }

        public DateTime? Revoked { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpiresIn;
        public bool IsRevoked => Revoked != null;
        public bool IsActive => !IsExpired && !IsRevoked;
    }
}
