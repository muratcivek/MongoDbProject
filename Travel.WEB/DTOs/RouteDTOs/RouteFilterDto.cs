namespace Travel.WEB.DTOs.RouteDTOs
{
    public class RouteFilterDto
    {
        public string? SearchText { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public string SortBy { get; set; } = "city";

        public bool Descending { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 5;
    }
}