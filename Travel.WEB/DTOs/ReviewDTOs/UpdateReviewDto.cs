namespace Travel.WEB.DTOs.ReviewDTOs
{
    public class UpdateReviewDto
    {
        public string Id { get; set; }

        public string RouteId { get; set; }

        public string UserName { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; }
    }
}