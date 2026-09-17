using Travel.WEB.DTOs.ReviewDTOs;
using Travel.WEB.DTOs.RouteDTOs;

namespace Travel.WEB.Models.Admin
{
    public class RouteUpdateViewModel
    {
        public UpdateRouteDto Route { get; set; } = new();

        public List<ResultReviewDto> Reviews { get; set; } = new();
    }
}