using AutoMapper;

namespace Travel.WEB.Mappings
{
    public class RouteMappings : Profile

    {
        public RouteMappings()
        {
            CreateMap<Entities.Route, DTOs.RouteDTOs.ResultRouteDto>().ReverseMap();
            CreateMap<Entities.Route, DTOs.RouteDTOs.CreateRouteDto>().ReverseMap();
            CreateMap<Entities.Route, DTOs.RouteDTOs.UpdateRouteDto>().ReverseMap();
            CreateMap<DTOs.RouteDTOs.ResultRouteDto, DTOs.RouteDTOs.UpdateRouteDto>().ReverseMap();
        }

    }
}
