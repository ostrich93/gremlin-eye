using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace gremlin_eye.Server.Entity
{
    [PrimaryKey(nameof(GameId), nameof(CompanyId))]
    public class GameCompany
    {
        [Column(Order = 0)]
        public long GameId { get; set; }
        [Column(Order = 1)]
        public long CompanyId { get; set; }
        [ForeignKey(nameof(GameId))]
        public virtual GameData Game { get; set; }
        [ForeignKey(nameof(CompanyId))]
        public virtual CompanyData Company { get; set; }
    }
}
