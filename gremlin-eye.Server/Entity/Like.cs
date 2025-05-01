using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gremlin_eye.Server.Entity
{
    [Table("review_likes")]
    public class ReviewLike
    {
        [Key]
        [Column("like_id")]
        public long LikeId { get; set; }

        [Column("review_id")]
        public long ReviewId { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }
        
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public AppUser User { get; set; } = null!;
        public Review Review { get; set; } = null!;
    }

    [Table("listing_likes")]
    public class ListingLike
    {
        [Key]
        [Column("like_id")]
        public long LikeId { get; set; }

        [Column("listing_id")]
        public long ListingId { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public AppUser User { get; set; } = null!;
        public Listing Listing { get; set; } = null!;
    }

    [Table("game_likes")]
    public class GameLike
    {
        [Key]
        [Column("like_id")]
        public long LikeId { get; set; }

        [Column("game_id")]
        public long GameId { get; set; }

        [Column("game_slug")]
        public string GameSlug { get; set; } = string.Empty;

        [Column("user_id")]
        public Guid UserId { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public AppUser User { get; set; } = null!;
        public GameData Game { get; set; } = null!;
    }
}
