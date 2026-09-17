namespace Travel.WEB.DTOs.ReviewDTOs
{
    public class ResultReviewDto
    {
        public string Id { get; set; }

        public string RouteId { get; set; }

        public string UserName { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}