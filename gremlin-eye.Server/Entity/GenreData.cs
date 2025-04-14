using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gremlin_eye.Server.Entity
{
    [Table("genres")]
    public class GenreData
    {
        [Key]
        [Column("genre_id")]
        public long GenreId { get; set; }

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("slug")]
        public string Slug { get; set; } = string.Empty;
        //Navigation Properties
        public virtual ICollection<GameData> Games { get; set; } = new List<GameData>();
    }
}
