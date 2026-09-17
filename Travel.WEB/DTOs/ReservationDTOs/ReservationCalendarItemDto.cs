namespace Travel.WEB.DTOs.ReservationDTOs
{
    public class ReservationCalendarItemDto
    {
        public DateTime TravelDate { get; set; }

        public string RouteId { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public int ReservationCount { get; set; }

        public int PersonCount { get; set; }
    }
}