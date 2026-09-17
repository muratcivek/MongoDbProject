using Travel.WEB.DTOs.ReservationDTOs;
using Travel.WEB.DTOs.RouteDTOs;

namespace Travel.WEB.Models.Admin
{
    public class ReservationListItemViewModel
    {
        public ResultReservationDto Reservation { get; set; } = new();

        public ResultRouteDto? Route { get; set; }
    }
}