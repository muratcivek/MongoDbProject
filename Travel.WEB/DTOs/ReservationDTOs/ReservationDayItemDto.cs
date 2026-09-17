namespace Travel.WEB.DTOs.ReservationDTOs
{
    public class ReservationDayItemDto
    {
        public string Id { get; set; }

        public string RouteId { get; set; }

        public string ReservationCode { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public int PersonCount { get; set; }

        public DateTime TravelDate { get; set; }

        public string Status { get; set; }
    }
}