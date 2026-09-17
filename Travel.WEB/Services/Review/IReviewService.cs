using Travel.WEB.DTOs.ReviewDTOs;

namespace Travel.WEB.Services.Review
{
    public interface IReviewService
    {
        Task<List<ResultReviewDto>> GetAllAsync();

        Task<List<ResultReviewDto>>
            GetByRouteIdAsync(string routeId);

        Task<ResultReviewDto?>
            GetByIdAsync(string id);

        Task CreateAsync(
            CreateReviewDto createReviewDto);

        Task UpdateAsync(
            UpdateReviewDto updateReviewDto);

        Task DeleteAsync(string id);

        Task CreateIndexesAsync();
    }
}