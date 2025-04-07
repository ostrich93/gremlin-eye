using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gremlin_eye.Server.Models
{
    [Table("reviews")]
    public class Review
    {

        [Key]
        [Column("review_id")]
        public int ReviewId { get; set; }

        [Column("game_id")]
        public int GameId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; } //Required Foreign Key property

        [Column("playthrough_id")]
        public int PlaythroughId { get; set; } //Required Foreign Key property

        [Column("comment_text")]
        public string ReviewText { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("comments_locked")]
        public bool CommentsLocked { get; set; } = false;

        //Navigation Properties
        public User User { get; set; } = null!;
        public Playthrough Playthrough { get; set; } = null!;
        public ICollection<ReviewComment> Comments { get; set; } = new List<ReviewComment>();
        public ICollection<ReviewLike> Likes { get; set; } = new List<ReviewLike>();
    }
}
