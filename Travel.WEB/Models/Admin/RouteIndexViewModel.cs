using Travel.WEB.DTOs.Common;
using Travel.WEB.DTOs.RouteDTOs;

namespace Travel.WEB.Models.Admin
{
    public class RouteIndexViewModel
    {
        public RouteFilterDto Filter { get; set; } = new();

        public PagedResultDto<ResultRouteDto> Routes { get; set; } = new();
    }
}