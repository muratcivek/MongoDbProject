using Travel.WEB.DTOs.ReservationDTOs;
using Travel.WEB.DTOs.ReviewDTOs;
using Travel.WEB.DTOs.RouteDTOs;

namespace Travel.WEB.Models.Public
{
    public class TourDetailViewModel
    {
        public ResultRouteDto Route { get; set; } = new();

        public List<ResultReviewDto> Reviews { get; set; } = new();

        public CreateReviewDto ReviewForm { get; set; } = new();

        public CreateReservationDto ReservationForm { get; set; } = new();
    }
}