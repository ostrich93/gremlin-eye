namespace gremlin_eye.Server.DTOs
{
    public class SeriesDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public ICollection<GameSummaryDTO> Games { get; set; } = new List<GameSummaryDTO>();
    }
}
