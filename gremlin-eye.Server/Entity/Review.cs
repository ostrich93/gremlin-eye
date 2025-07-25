using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gremlin_eye.Server.Entity
{
    [Table("reviews")]
    public class Review
    {

        [Key]
        [Column("review_id")]
        public long Id { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; } //Required Foreign Key property

        [Column("playthrough_id")]
        public long PlaythroughId { get; set; } //Required Foreign Key property

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("comments_locked")]
        public bool CommentsLocked { get; set; } = false;

        //Navigation Properties
        public AppUser User { get; set; } = null!;
        public Playthrough Playthrough { get; set; } = null!;
        public virtual ICollection<ReviewComment> Comments { get; set; } = new List<ReviewComment>();
        public virtual ICollection<ReviewLike> Likes { get; set; } = new List<ReviewLike>();
    }
}
