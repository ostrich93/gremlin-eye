using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gremlin_eye.Server.Entity
{
    [Table("playthroughs")]
    public class Playthrough
    {

        [Key]
        [Column("playthrough_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("game_log_id")]
        public long GameLogId { get; set; } // Required foreign key property

        [Column("game_id")]
        public long GameId { get; set; }

        [Column("log_title")]
        public string? LogTitle { get; set; } = string.Empty;

        [Column("time_played")]
        public TimeSpan? TimePlayed { get; set; }

        [Column("is_mastered")]
        public bool IsMastered { get; set; } = false;

        [Column("is_replay")]
        public bool IsReplay { get; set; } = false;

        [Column("medium")]
        public string? Medium { get; set; }

        [Column("played_on")]
        public long? PlatformId { get; set; }

        [Column("review")]
        public string ReviewText { get; set; } = string.Empty;

        [Column("review_spoilers")]
        public bool ReviewSpoilers { get; set; } = false;

        [Column("rating")]
        [Range(minimum: 0, maximum: 10)]
        public int Rating { get; set; } = 0; //a rating of 0 is equivalent to unrated

        //Navigation Properties
        public GameData Game { get; set; } = null!;
        public GameLog GameLog { get; set; } = null!;
        public virtual Review? Review { get; set; }
        public virtual ICollection<PlayLog> PlayLogs { get; set; } = new List<PlayLog>();
        public virtual PlatformData? Platform { get; set; }
    }
}
