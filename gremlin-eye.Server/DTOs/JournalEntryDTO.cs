namespace gremlin_eye.Server.DTOs
{
    public class JournalEntryDTO
    {
        public long PlayLogId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int Hours { get; set; } = 0;
        public int Minutes { get; set; } = 0;
        public bool IsStart { get; set; } = false;
        public bool IsEnd { get; set; } = false;
        public string? LogNote { get; set; }
    }
}
