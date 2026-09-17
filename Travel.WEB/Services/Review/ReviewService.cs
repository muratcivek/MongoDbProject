using AutoMapper;
using MongoDB.Driver;
using Travel.WEB.DTOs.ReviewDTOs;
using Travel.WEB.Settings;

namespace Travel.WEB.Services.Review
{
    public class ReviewService : IReviewService
    {
        private readonly IMongoCollection<Entities.Review>
            _reviewCollection;

        private readonly IMapper _mapper;

        public ReviewService(
            IDatabaseSettings databaseSettings,
            IMapper mapper)
        {
            var client =
                new MongoClient(
                    databaseSettings.ConnectionString);

            var database =
                client.GetDatabase(
                    databaseSettings.DatabaseName);

            _reviewCollection =
                database.GetCollection<Entities.Review>(
                    databaseSettings.ReviewCollectionName);

            _mapper = mapper;
        }


        public async Task CreateAsync(
            CreateReviewDto createReviewDto)
        {
            var review =
                _mapper.Map<Entities.Review>(
                    createReviewDto);

            review.CreatedDate =
                DateTime.UtcNow;

            await _reviewCollection
                .InsertOneAsync(review);
        }


        public async Task DeleteAsync(string id)
        {
            await _reviewCollection
                .DeleteOneAsync(
                    x => x.Id == id);
        }


        public async Task<List<ResultReviewDto>>
            GetAllAsync()
        {
            var reviews =
                await _reviewCollection
                    .Find(_ => true)
                    .SortByDescending(
                        x => x.CreatedDate)
                    .ToListAsync();

            return _mapper.Map<
                List<ResultReviewDto>>(reviews);
        }


        public async Task<List<ResultReviewDto>>
            GetByRouteIdAsync(string routeId)
        {
            var reviews =
                await _reviewCollection
                    .Find(
                        x => x.RouteId == routeId)
                    .SortByDescending(
                        x => x.CreatedDate)
                    .ToListAsync();

            return _mapper.Map<
                List<ResultReviewDto>>(reviews);
        }


        public async Task<ResultReviewDto?>
            GetByIdAsync(string id)
        {
            var review =
                await _reviewCollection
                    .Find(x => x.Id == id)
                    .FirstOrDefaultAsync();

            if (review == null)
                return null;

            return _mapper.Map<ResultReviewDto>(
                review);
        }

        //admin yorum ekleme güncelleme kaldırıldı 
        public async Task UpdateAsync(
            UpdateReviewDto dto)
        {
            var filter =
                Builders<Entities.Review>
                    .Filter
                    .Eq(x => x.Id, dto.Id);

            var update =
                Builders<Entities.Review>
                    .Update
                    .Set(
                        x => x.UserName,
                        dto.UserName)
                    .Set(
                        x => x.Rating,
                        dto.Rating)
                    .Set(
                        x => x.Comment,
                        dto.Comment);

            await _reviewCollection
                .UpdateOneAsync(
                    filter,
                    update);
        }

        public async Task CreateIndexesAsync()
        {
            var index =
                new CreateIndexModel<Entities.Review>(
                    Builders<Entities.Review>
                        .IndexKeys
                        .Ascending(x => x.RouteId)
                        .Descending(x => x.CreatedDate),

                    new CreateIndexOptions
                    {
                        Name = "idx_review_routeId_createdDate"
                    });

            await _reviewCollection
                .Indexes
                .CreateOneAsync(index);
        }
    }
}