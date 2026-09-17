using Travel.WEB.DTOs.Common;
using Travel.WEB.DTOs.RouteDTOs;

namespace Travel.WEB.Services.Route
{
    public interface IRouteService
    {
        Task<List<ResultRouteDto>> GetAllByCityAsync(string city);
        Task<List<ResultRouteDto>> GetAllAsync();
        Task<ResultRouteDto> GetByIdAsync(string id);
        Task CreateAsync(CreateRouteDto createRouteDto);
        Task UpdateAsync(UpdateRouteDto updateRouteDto);
        Task DeleteAsync(string id);
        Task<PagedResultDto<ResultRouteDto>> GetFilteredAsync(
    RouteFilterDto filterDto);
        Task CreateIndexesAsync();
        Task AddFeatureAsync(string routeId, string feature);
        Task RemoveFeatureAsync(string routeId, string feature);
    }
}
