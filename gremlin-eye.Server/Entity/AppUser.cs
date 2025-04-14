using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace gremlin_eye.Server.Entity
{
    [Table("users")]
    public class AppUser : IdentityUser
    {

        [Column("avatar_url")]
        public string? AvatarUrl { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //[Column("role")]
        //[DefaultValue("user")]
        //public string Role { get; set; } = "user";

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
