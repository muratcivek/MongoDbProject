using AutoMapper;
using MongoDB.Driver;
using Travel.WEB.DTOs.BannerDTOs;
using Travel.WEB.Entities;
using Travel.WEB.Settings;

namespace Travel.WEB.Services.Banner
{
    public class BannerService : IBannerService
    {
        private readonly IMongoCollection<Travel.WEB.Entities.Banner> _bannerCollection;
        private readonly IMapper _mapper;

        public BannerService(IDatabaseSettings databaseSettings, IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _bannerCollection =
                database.GetCollection<Travel.WEB.Entities.Banner>(
                    databaseSettings.BannerCollectionName);

            _mapper = mapper;
        }

        public async Task CreateAsync(CreateBannerDto createBannerDto)
        {
            var banner = _mapper.Map<Travel.WEB.Entities.Banner>(createBannerDto);

            await _bannerCollection.InsertOneAsync(banner);
        }

        public async Task DeleteAsync(string id)
        {
            await _bannerCollection.DeleteOneAsync(b => b.Id == id);
        }

        public async Task<List<ResultBannerDto>> GetAllAsync()
        {
            var banners = await _bannerCollection
                  .Find(x => true)
                  .ToListAsync();

            return _mapper.Map<List<ResultBannerDto>>(banners);
        }

        public async Task<ResultBannerDto> GetByIdAsync(string id)
        {
            var banner = await _bannerCollection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            return _mapper.Map<ResultBannerDto>(banner);
        }

        public async Task UpdateAsync(UpdateBannerDto updateBannerDto)
        {
            var banner = _mapper.Map<Travel.WEB.Entities.Banner>(updateBannerDto);

            await _bannerCollection
                .ReplaceOneAsync(x => x.Id == updateBannerDto.Id, banner);
        }
    }
}