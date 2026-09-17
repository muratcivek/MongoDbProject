using Travel.WEB.DTOs.ReservationDTOs;
using Travel.WEB.DTOs.RouteDTOs;

namespace Travel.WEB.Models.Public
{
    public class ReservationTrackViewModel
    {
        public TrackReservationDto Search { get; set; } = new();

        public ResultReservationDto? Reservation { get; set; }

        public ResultRouteDto? Route { get; set; }

        public bool Searched { get; set; }
    }
}