using Travel.WEB.Entities.Common;

namespace Travel.WEB.Entities
{
    public class Reservation : BaseEntity
    {
        public string RouteId { get; set; }

        public string ReservationCode { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public int PersonCount { get; set; }

        public DateTime TravelDate { get; set; }

        public DateTime CreatedDate { get; set; }
            = DateTime.UtcNow;

        public string Status { get; set; }
            = "Pending";
    }
}