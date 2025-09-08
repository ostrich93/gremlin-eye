using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace gremlin_eye.Server.Entity
{
    [PrimaryKey(nameof(GameId), nameof(PlatformId))]
    public class GamePlatform
    {
        [Column(Order = 0)]
        public long GameId { get; set; }
        [Column(Order = 1)]
        public long PlatformId { get; set; }
        [ForeignKey(nameof(GameId))]
        public virtual GameData Game { get; set; }
        [ForeignKey(nameof(PlatformId))]
        public virtual PlatformData Platform { get; set; }
    }
}
