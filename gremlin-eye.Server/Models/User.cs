using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gremlin_eye.Server.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("user_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        [Required]
        [EmailAddress]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Column("password")]
        public string Password { get; set; } = string.Empty;

        [Column("avatar_url")]
        public string? AvatarUrl { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("last_login")]
        public DateTime? LastLogin { get; set; }

        [Column("role")]
        [DefaultValue("user")]
        public string Role { get; set; } = "user";

        //Navigation Properties
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
        public ICollection<GameLog> GameLogs { get; set; } = new List<GameLog>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Listing> Listings { get; set; } = new List<Listing>();
        public ICollection<ReviewComment> ReviewComments { get; set; } = new List<ReviewComment>();
        public ICollection<ListingComment> ListingComments { get; set; } = new List<ListingComment>();
        public ICollection<GameLike> GameLikes { get; set; } = new List<GameLike>();
        public ICollection<ReviewLike> ReviewLikes { get; set; } = new List<ReviewLike>();
        public ICollection<ListingLike> ListingLikes { get; set; } = new List<ListingLike>();
    }
}
