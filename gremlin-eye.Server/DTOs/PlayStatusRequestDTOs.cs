using gremlin_eye.Server.Enums;

namespace gremlin_eye.Server.DTOs
{
    public class UpdateGameLogTypeRequest
    {
        public PlayingType Type { get; set; }
        public long GameId { get; set; }
    }

    public class UpdatePlayStatusRequest
    {
        public PlayState Status { get; set; }
        public long GameId { get; set; }
    }
}
