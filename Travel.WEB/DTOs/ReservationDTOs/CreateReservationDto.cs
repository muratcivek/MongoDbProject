namespace Travel.WEB.DTOs.ReservationDTOs
{
    public class CreateReservationDto
    {
        public string RouteId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public int PersonCount { get; set; }

        public DateTime TravelDate { get; set; }
    }
}