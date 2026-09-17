namespace Travel.WEB.DTOs.Common
{
    public class PagedResultDto<T>
    {
        public List<T> Items { get; set; } = new();

        public long TotalCount { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int TotalPages =>
            PageSize == 0
                ? 0
                : (int)Math.Ceiling(TotalCount / (double)PageSize); //toplam sayfa sayısı hesaplanır 

        public bool HasPreviousPage => Page > 1;

        public bool HasNextPage => Page < TotalPages;
    }
}