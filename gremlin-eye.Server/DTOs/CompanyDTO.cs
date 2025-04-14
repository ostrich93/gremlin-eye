namespace gremlin_eye.Server.DTOs
{
    public class CompanyDTO
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public ICollection<GameSummaryDTO>? Published { get; set; } //nullable list of games published
    }
}
