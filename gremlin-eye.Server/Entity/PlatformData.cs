using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gremlin_eye.Server.Entity
{
    [Table("platforms")]
    public class PlatformData
    {
        [Key]
        [Column("platform_id")]
        public long PlatformId { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("slug")]
        public string Slug { get; set; } = string.Empty;

        //Navigation Properties
        public virtual ICollection<GameData> Games { get; set; } = new List<GameData>();
        public virtual ICollection<Playthrough> Playthroughs { get; set; } = new List<Playthrough>();

    }
}
