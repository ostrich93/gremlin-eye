namespace gremlin_eye.Server.DTOs
{
    //used to populate the Journal Modal
    public class GameLogFormDTO : GameLogDTO
    {
        public string GameName { get; set; } = string.Empty;
        public string CoverUrl { get; set; } = string.Empty;
        public DateTimeOffset? ReleaseDate { get; set; }
        public ICollection<PlatformDTO> Platforms { get; set; } = new List<PlatformDTO>();
    }
}
