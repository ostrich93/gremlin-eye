using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace gremlin_eye.Server.Entity
{
    [Table("companies")]
    public class CompanyData
    {
        [Key]
        [Column("company_id")]
        public long CompanyId { get; set; }

        [Column("description")]
        public string Description { get; set; } = string.Empty;

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("slug")]
        public string Slug { get; set; } = string.Empty;

        //Navigation Properties
        public virtual ICollection<GameData> Games { get; set; } = new List<GameData>();
    }
}
