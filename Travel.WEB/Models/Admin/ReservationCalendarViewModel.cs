using Travel.WEB.DTOs.ReservationDTOs;

namespace Travel.WEB.Models.Admin
{
    public class ReservationCalendarViewModel
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public List<ReservationCalendarItemDto> Items
        {
            get;
            set;
        } = new();
    }
}