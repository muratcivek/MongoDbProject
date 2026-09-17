using AutoMapper;
using MongoDB.Driver;
using Travel.WEB.DTOs.Common;
using Travel.WEB.DTOs.RouteDTOs;
using Travel.WEB.Settings;

namespace Travel.WEB.Services.Route
{
    public class RouteService : IRouteService
    {
        private readonly IMongoCollection<Entities.Route> _routesCollection;
        private readonly IMapper _mapper;
        public RouteService(
      IMapper mapper,
      IDatabaseSettings databaseSettings)
        {
            _mapper = mapper;

            var client =
                new MongoClient(databaseSettings.ConnectionString);

            var database =
                client.GetDatabase(databaseSettings.DatabaseName);

            _routesCollection =
                database.GetCollection<Entities.Route>(
                    databaseSettings.RouteCollectionName
                );
        }

        public async Task CreateAsync(CreateRouteDto createRouteDto)
        {
            var route = _mapper.Map<Entities.Route>(createRouteDto);

            await _routesCollection.InsertOneAsync(route);
        }

        public async Task DeleteAsync(string id)
        {
            var filter = Builders<Entities.Route>.Filter.Eq(r => r.Id, id);

            await _routesCollection.DeleteOneAsync(filter);
        }

        public async Task<List<ResultRouteDto>> GetAllAsync()
        {
            var routes = await _routesCollection.Find(_ => true).ToListAsync(); 

            return _mapper.Map<List<ResultRouteDto>>(routes);
        }

        public async Task<List<ResultRouteDto>> GetAllByCityAsync(string city)
        {
            var filter = Builders<Entities.Route>.Filter.Regex(
                r => r.City,
                new MongoDB.Bson.BsonRegularExpression(city.Trim(), "i")
            );

            var routes = await _routesCollection
                .Find(filter)
                .ToListAsync();

            return _mapper.Map<List<ResultRouteDto>>(routes);
        }

        public async Task<ResultRouteDto?> GetByIdAsync(string id)
        {
            var route =
                await _routesCollection
                    .Find(x => x.Id == id)
                    .FirstOrDefaultAsync();

            if (route == null)
            {
                return null;
            }

            return _mapper.Map<ResultRouteDto>(route);
        }

        public async Task UpdateAsync(UpdateRouteDto dto)
        {
            var filter = Builders<Entities.Route>
                .Filter
                .Eq(x => x.Id, dto.Id);

            var update = Builders<Entities.Route>
                .Update
                .Set(x => x.City, dto.City)
                .Set(x => x.Country, dto.Country)
                .Set(x => x.Duration, dto.Duration)
                .Set(x => x.ImageUrl, dto.ImageUrl)
                .Set(x => x.Price, dto.Price)
                .Set(
                    x => x.Details.Transportation,
                    dto.Details.Transportation)
                .Set(
                    x => x.Details.Accommodation,
                    dto.Details.Accommodation)
                .Set(
                    x => x.Details.Description,
                    dto.Details.Description);

            await _routesCollection.UpdateOneAsync(
                filter,
                update);
        }

        public async Task<PagedResultDto<ResultRouteDto>> GetFilteredAsync(
    RouteFilterDto filterDto)
        {
            var filterBuilder = Builders<Entities.Route>.Filter;

            var filter = filterBuilder.Empty;

            if (!string.IsNullOrWhiteSpace(filterDto.SearchText))
            {
                filter &= filterBuilder.Text(
                    filterDto.SearchText.Trim());
            }

            if (!string.IsNullOrWhiteSpace(filterDto.City))
            {
                filter &= filterBuilder.Eq(
                    x => x.City,
                    filterDto.City.Trim()
                );
            }

            if (!string.IsNullOrWhiteSpace(filterDto.Country))
            {
                filter &= filterBuilder.Eq(
                    x => x.Country,
                    filterDto.Country.Trim()
                );
            }

            if (filterDto.MinPrice.HasValue)
            {
                filter &= filterBuilder.Gte(
                    x => x.Price,
                    filterDto.MinPrice.Value
                );
            }

            if (filterDto.MaxPrice.HasValue)
            {
                filter &= filterBuilder.Lte(
                    x => x.Price,
                    filterDto.MaxPrice.Value
                );
            }

            var totalCount = await _routesCollection
                .CountDocumentsAsync(filter);

            var query = _routesCollection.Find(filter);

            query = filterDto.SortBy?.ToLower() switch
            {
                "price" when filterDto.Descending =>
                    query.SortByDescending(x => x.Price),

                "price" =>
                    query.SortBy(x => x.Price),

                "country" when filterDto.Descending =>
                    query.SortByDescending(x => x.Country),

                "country" =>
                    query.SortBy(x => x.Country),

                "city" when filterDto.Descending =>
                    query.SortByDescending(x => x.City),

                _ =>
                    query.SortBy(x => x.City)
            };

            var page = filterDto.Page < 1
                ? 1
                : filterDto.Page;

            var pageSize = filterDto.PageSize switch
            {
                < 1 => 5,
                > 50 => 50,
                _ => filterDto.PageSize
            };

            var routes = await query
                .Skip((page - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return new PagedResultDto<ResultRouteDto>
            {
                Items = _mapper.Map<List<ResultRouteDto>>(routes),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task CreateIndexesAsync()
        {
            var indexModels = new List<CreateIndexModel<Entities.Route>>
    {
        new(
            Builders<Entities.Route>
                .IndexKeys
                .Ascending(x => x.City),

            new CreateIndexOptions
            {
                Name = "idx_route_city"
            }
        ),

        new(
            Builders<Entities.Route>
                .IndexKeys
                .Ascending(x => x.Country),

            new CreateIndexOptions
            {
                Name = "idx_route_country"
            }
        ),

        new(
            Builders<Entities.Route>
                .IndexKeys
                .Ascending(x => x.Price),

            new CreateIndexOptions
            {
                Name = "idx_route_price"
            }
        ),

        new(
            Builders<Entities.Route>
                .IndexKeys
                .Ascending(x => x.Country)
                .Ascending(x => x.City)
                .Ascending(x => x.Price),

            new CreateIndexOptions
            {
                Name = "idx_route_country_city_price"
            }
        ),

new(
    Builders<Entities.Route>
        .IndexKeys
        .Text(x => x.City)
        .Text(x => x.Country)
        .Text(x => x.Duration)
        .Text(x => x.Details.Description),

    new CreateIndexOptions
    {
        Name = "idx_route_text_search"
    }
)
    };


            await _routesCollection.Indexes
                .CreateManyAsync(indexModels);
        }

        public async Task AddFeatureAsync(
    string routeId,
    string feature)
        {
            var filter = Builders<Entities.Route>
                .Filter
                .Eq(x => x.Id, routeId);

            var update = Builders<Entities.Route>
                .Update
                .AddToSet(
                    x => x.Features,
                    feature);

            await _routesCollection.UpdateOneAsync(
                filter,
                update);
        }
        public async Task RemoveFeatureAsync(
    string routeId,
    string feature)
        {
            var filter = Builders<Entities.Route>
                .Filter
                .Eq(x => x.Id, routeId);

            var update = Builders<Entities.Route>
                .Update
                .Pull(
                    x => x.Features,
                    feature);

            await _routesCollection.UpdateOneAsync(
                filter,
                update);
        }
    }
}
