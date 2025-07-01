namespace gremlin_eye.Server.DTOs
{
    public class PlaythroughDTO
    {
        public long LogId { get; set; }
        public long GameId { get; set; }
        public long PlaythroughId { get; set; }
        public string LogTitle { get; set; } = string.Empty;
        public bool IsReplay { get; set; } = false;
        public string? Medium { get; set; }
        public PlatformDTO? Platform { get; set; }
        public string ReviewText { get; set; } = string.Empty;
        public bool ContainsSpoilers { get; set; } = false;
        public int? Rating { get; set; }
        public long? ReviewId { get; set; }
        public ICollection<JournalEntryDTO> PlayLogs { get; set; } = new List<JournalEntryDTO>();
        public ICollection<long> PlayLogsToDelete { get; set; } = new List<long>();
    }
}
