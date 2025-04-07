using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gremlin_eye.Server.Models
{
    [Table("game_logs")]
    public class GameLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("game_log_id")]
        public int GameLogId { get; set; }

        [Key]
        [Required]
        [Column("game_id")]
        public int GameId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; } //required foreign key

        [Column("play_status")]
        public PlayState? PlayStatus { get; set; }

        [Column("is_played")]
        [DefaultValue(false)]
        public bool IsPlayed { get; set; } = false;

        [Column("is_playing")]
        [DefaultValue(false)]
        public bool IsPlaying { get; set; } = false;

        [Column("is_backlog")]
        [DefaultValue(false)]
        public bool IsBacklog { get; set; } = false;

        [Column("is_wishlist")]
        [DefaultValue(false)]
        public bool IsWishlist { get; set; } = false;

        //Navigation Properties
        public User User { get; set; } = null!;
        public GameLike? Like { get; set; }
        public ICollection<Playthrough> Playthroughs { get; set; } = new List<Playthrough>();

    }
}
