using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gremlin_eye.Server.Entity
{
    [Table("play_logs")]
    public class PlayLog
    {

        [Key]
        [Column("play_log_id")]
        public long PlayLogId { get; set; }

        [Column("playthrough_id")]
        public long PlaythroughId { get; set; } //required foreign key

        [Column("log_time")]
        public TimeSpan? LogTime { get; set; } //time played

        [Column("log_date")]
        public DateOnly LogDate { get; set; }

        [Column("is_start")]
        public bool IsStart { get; set; } = false;

        [Column("is_end")]
        public bool IsEnd { get; set; } = false;

        [Column("log_note")]
        public string? LogNote { get; set; }

        //Navigation Properties
        public Playthrough Playthrough { get; set; } = null!;
    }
}
