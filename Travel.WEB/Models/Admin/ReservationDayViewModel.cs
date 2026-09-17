using Travel.WEB.DTOs.ReservationDTOs;
using Travel.WEB.DTOs.RouteDTOs;

namespace Travel.WEB.Models.Admin
{
    public class ReservationDayItemViewModel
    {
        public ResultReservationDto Reservation
        {
            get;
            set;
        } = new();

        public ResultRouteDto? Route
        {
            get;
            set;
        }
    }


    public class ReservationDayViewModel
    {
        public DateTime Date { get; set; }

        public List<ReservationDayItemViewModel> Items
        {
            get;
            set;
        } = new();
    }
}