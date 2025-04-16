using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gremlin_eye.Server.Entity
{
    [Table("users")]
    public class AppUser
    {

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [Column("user_name")]
        [StringLength(16, MinimumLength = 2)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string PasswordHash { get; set; } = string.Empty;

        public byte[] Salt { get; set; } = new byte[100];

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Column("avatar_url")]
        public string? AvatarUrl { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("role")]
        [Required]
        public UserRole Role { get; set; }

        //Navigation Properties
        public virtual ICollection<GameLog> GameLogs { get; set; } = new List<GameLog>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<Listing> Listings { get; set; } = new List<Listing>();
        public virtual ICollection<ReviewComment> ReviewComments { get; set; } = new List<ReviewComment>();
        public virtual ICollection<ListingComment> ListingComments { get; set; } = new List<ListingComment>();
        public virtual ICollection<GameLike> GameLikes { get; set; } = new List<GameLike>();
        public virtual ICollection<ReviewLike> ReviewLikes { get; set; } = new List<ReviewLike>();
        public virtual ICollection<ListingLike> ListingLikes { get; set; } = new List<ListingLike>();
    }
}
