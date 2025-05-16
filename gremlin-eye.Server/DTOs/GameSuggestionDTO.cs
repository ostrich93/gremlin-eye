namespace gremlin_eye.Server.DTOs
{
    public class GameSuggestionDTO
    {
        public long Id { get; set; }
        public string Value { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public int? Year { get; set; }
    }
}
