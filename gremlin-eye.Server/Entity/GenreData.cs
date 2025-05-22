using gremlin_eye.Server.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gremlin_eye.Server.Entity
{
    [Table("genres")]
    public class GenreData : IChecksum
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("genre_id")]
        public long Id { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("slug")]
        public string Slug { get; set; } = string.Empty;
        [Column("checksum")]
        public string? Checksum { get; set; }
        //Navigation Properties
        public virtual ICollection<GameData> Games { get; set; } = new List<GameData>();
    }
}
