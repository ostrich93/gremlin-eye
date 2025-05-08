using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gremlin_eye.Server.Entity
{

    [Table("listings")]
    public class Listing
    {

        [Key]
        [Column("listing_id")]
        public long Id { get; set; }

        [Column("user_id")]
        public Guid UserId { get; set; } //required foreign key

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("slug")]
        public string Slug { get; set; } = string.Empty;

        [Column("description")]
        public string? Description { get; set; }

        [Column("is_public")]
        public bool IsPublic { get; set; } = true;

        [Column("is_ranked")]
        public bool isRanked { get; set; } = false;

        [Column("is_grid")]
        public bool isGrid { get; set; } = false;

        [Column("default_sorting")]
        [DefaultValue("userorder")]
        public string DefaultSorting = "userorder";

        [Column("is_desc")]
        public bool isDesc { get; set; } = true;

        [Column("comments_locked")]
        public bool CommentsLocked { get; set; } = false;

        //Navigation Properties
        public virtual AppUser User { get; set; } = null!;
        public virtual ICollection<ListEntry> ListEntries { get; set; } = new List<ListEntry>();
        public virtual ICollection<ListingComment> Comments { get; set; } = new List<ListingComment>();
        public virtual ICollection<ListingLike> Likes { get; set; } = new List<ListingLike>();
    }
}
