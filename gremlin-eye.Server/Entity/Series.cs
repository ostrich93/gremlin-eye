using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gremlin_eye.Server.Entity
{
    [Table("game_series")]
    public class Series
    {
        [Key]
        [Column("series_id")]
        public long SeriesId { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("slug")]
        public string Slug { get; set; } = string.Empty;

        //Navigation Properties
        public virtual ICollection<GameData> Games { get; set; } = new List<GameData>();
    }
}
