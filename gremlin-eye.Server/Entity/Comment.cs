using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gremlin_eye.Server.Entity
{

    //A comment ALWAYS belongs to a User. They can belong to either a Listing OR a Review, but not both, and the association is required.
    //One approach I toyed with was having an enum for the model type (Listing, Review) and a modelId that would be used in queries to pull from the appropriate table. However, this seemed more trouble than it was worth.

    [Table("review_comments")]
    public class ReviewComment
    {
        [Key]
        [Column("author_id")]
        public long AuthorId { get; set; }

        [Key]
        [Column("review_id")]
        public long ReviewId { get; set; }

        [Column("comment_body")]
        public string CommentBody { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public User Author { get; set; } = null!;

        public Review Review { get; set; } = null!;
    }

    [Table("listing_comments")]
    public class ListingComment
    {
        [Key]
        [Column("author_id")]
        public long AuthorId { get; set; }

        [Key]
        [Column("listing_id")]
        public long ListingId { get; set; }

        [Column("comment_body")]
        public string CommentBody { get; set; } = string.Empty;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        public User Author { get; set; } = null!;

        public Listing Listing { get; set; } = null!;
    }

}
