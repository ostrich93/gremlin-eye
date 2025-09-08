using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace gremlin_eye.Server.Entity
{
    [PrimaryKey(nameof(GameId), nameof(SeriesId))]
    public class GameSeries
    {
        [Column(Order = 0)]
        public long GameId { get; set; }
        [Column(Order = 1)]
        public long SeriesId { get; set; }
        [ForeignKey(nameof(GameId))]
        public virtual GameData Game { get; set; }
        [ForeignKey(nameof(SeriesId))]
        public virtual SeriesData Series { get; set; }
    }
}
