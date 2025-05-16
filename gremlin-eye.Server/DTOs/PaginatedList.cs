namespace gremlin_eye.Server.DTOs
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int PageNumber { get; set; }
        public int PageLimit { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageLimit);

        public int NextPage => PageNumber < PageLimit ? PageNumber + 1 : PageNumber;
        public bool HasNextPage => PageNumber < TotalPages;
        public int PreviousPage => PageNumber > 1 ? PageNumber - 1 : PageNumber;
        public bool HasPreviousPage => PageNumber > 1;
    }
}
