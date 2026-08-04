using Travel.WEB.DTOs.BannerDTOs;

namespace Travel.WEB.Services.Banner
{
    public interface IBannerService
    {
        Task<List<ResultBannerDto>> GetAllAsync();
        Task<ResultBannerDto> GetByIdAsync(string id);
        Task CreateAsync(CreateBannerDto createBannerDto);
        Task DeleteAsync(string id);
        Task UpdateAsync(UpdateBannerDto updateBannerDto);
    }
}
