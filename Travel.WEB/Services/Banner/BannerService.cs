using MongoDB.Driver;
using Travel.WEB.DTOs.BannerDTOs;
using Travel.WEB.Entities;
using Travel.WEB.Settings;

namespace Travel.WEB.Services.Banner
{
    public class BannerService : IBannerService
    {
        private readonly IMongoCollection<Travel.WEB.Entities.Banner> _bannerCollection;

        public BannerService(IDatabaseSettings databaseSettings )
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _bannerCollection = database.GetCollection<Travel.WEB.Entities.Banner>(databaseSettings.BannerCollectionName);
        }

        Task IBannerService.CreateAsync(CreateBannerDto createBannerDto)
        {
            throw new NotImplementedException();
        }

        Task IBannerService.DeleteAsync(string id)
        {
            throw new NotImplementedException();
        }

        Task<List<ResultBannerDto>> IBannerService.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<ResultBannerDto> IBannerService.GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        Task IBannerService.UpdateAsync(UpdateBannerDto updateBannerDto)
        {
            throw new NotImplementedException();
        }
    }
}
