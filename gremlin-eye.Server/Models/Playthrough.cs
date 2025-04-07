using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gremlin_eye.Server.Models
{
    [Table("playthroughs")]
    public class Playthrough
    {

        [Key]
        [Column("playthrough_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlaythroughId { get; set; }

        [Column("game_log_id")]
        public int GameLogId { get; set; } // Required foreign key property

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
        public int? PlatformId { get; set; }

        [Column("rating")]
        [Range(minimum:0, maximum:10)]
        public int? Rating { get; set; }

        //Navigation Properties
        public GameLog GameLog { get; set; } = null!;
        public Review? Review { get; set; }
        public ICollection<PlayLog> PlayLogs { get; set; } = new List<PlayLog>();
    }
}
