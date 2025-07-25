using gremlin_eye.Server.Enums;

namespace gremlin_eye.Server.DTOs
{
    public class GameLogDTO
    {
        public long LogId { get; set; }
        public long GameId { get; set; }
        public int? Rating { get; set; }
        public PlayState? PlayStatus { get; set; }
        public bool IsPlayed { get; set; }
        public bool IsPlaying { get; set; }
        public bool IsBacklog { get; set; }
        public bool IsWishlist { get; set; }
        public ICollection<PlaythroughDTO> Playthroughs { get; set; } = new List<PlaythroughDTO>();
        public ICollection<long> PlaythroughsToDelete { get; set; } = new List<long>();
    }
}
