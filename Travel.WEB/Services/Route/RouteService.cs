using AutoMapper;
using MongoDB.Driver;
using Travel.WEB.DTOs.RouteDTOs;
using Travel.WEB.Settings;

namespace Travel.WEB.Services.Route
{
    public class RouteService : IRouteService
    {
        private readonly IMongoCollection<Entities.Route> _routesCollection;
        private readonly IMapper mapper;

        public RouteService(IMapper mapper, IDatabaseSettings databaseSettings)
        {
            this.mapper = mapper;

            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _routesCollection =
                database.GetCollection<Travel.WEB.Entities.Route>(
                    databaseSettings.RouteCollectionName);
        }

        public async Task CreateAsync(CreateRouteDto createRouteDto)
        {
            var route = mapper.Map<Entities.Route>(createRouteDto);

            await _routesCollection.InsertOneAsync(route);
        }

        public async Task DeleteAsync(string id)
        {
            await _routesCollection.DeleteOneAsync(id);
        }

        public async Task<List<ResultRouteDto>> GetAllAsync()
        {
            var routes = await _routesCollection.Find(_ => true).ToListAsync(); 

            return mapper.Map<List<ResultRouteDto>>(routes);
        }

        public async Task<List<ResultRouteDto>> GetAllByCityAsync(string city)
        {
            var routes = await _routesCollection.Find(r => r.City == city).ToListAsync();

            return mapper.Map<List<ResultRouteDto>>(routes);
        }

        public Task<ResultRouteDto> GetByIdAsync(string id)
        {
           var route = _routesCollection.AsQueryable().FirstOrDefault(r => r.Id == id);
            if (route == null)
            {
                return Task.FromResult<ResultRouteDto>(null);
            }
            var resultRouteDto = mapper.Map<ResultRouteDto>(route);
            return Task.FromResult(resultRouteDto);
        }

        public async Task UpdateAsync(UpdateRouteDto updateRouteDto)
        {
            var route = mapper.Map<Entities.Route>(updateRouteDto);
            await _routesCollection.ReplaceOneAsync(r => r.Id == route.Id, route);

        }
    }
}
