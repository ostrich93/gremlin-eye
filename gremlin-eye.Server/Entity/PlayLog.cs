using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gremlin_eye.Server.Entity
{
    [Table("play_logs")]
    public class PlayLog
    {

        [Key]
        [Column("play_log_id")]
        public long Id { get; set; }

        [Column("playthrough_id")]
        public long PlaythroughId { get; set; } //required foreign key

        [Column("start_date")]
        public DateOnly StartDate { get; set; }

        [Column("end_date")]
        public DateOnly EndDate { get; set; } //by default, it's the same as start date

        [Column("hours")]
        public int Hours { get; set; } = 0;

        [Column("minutes")]
        public int Minutes { get; set; } = 0;

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
