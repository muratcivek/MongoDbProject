using MongoDB.Driver;
using Travel.WEB.Settings;

namespace Travel.WEB.Seeders
{
    public class RouteSeeder
    {
        private readonly IMongoCollection<Entities.Route> _routeCollection;

        public RouteSeeder(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(
                databaseSettings.ConnectionString);

            var database = client.GetDatabase(
                databaseSettings.DatabaseName);

            _routeCollection =
                database.GetCollection<Entities.Route>(
                    databaseSettings.RouteCollectionName);
        }

        public async Task SeedAsync(int count = 10000)
        {
            var existingCount =
                await _routeCollection.CountDocumentsAsync(
                    Builders<Entities.Route>.Filter.Empty);

            if (existingCount >= count)
            {
                return;
            }

            var countries = new Dictionary<string, string[]>
            {
                {
                    "Türkiye",
                    new[]
                    {
                        "İstanbul",
                        "Ankara",
                        "Eskişehir",
                        "İzmir",
                        "Antalya"
                    }
                },

                {
                    "Fransa",
                    new[]
                    {
                        "Paris",
                        "Lyon",
                        "Nice",
                        "Marsilya"
                    }
                },

                {
                    "İtalya",
                    new[]
                    {
                        "Roma",
                        "Milano",
                        "Venedik",
                        "Floransa"
                    }
                },

                {
                    "Almanya",
                    new[]
                    {
                        "Berlin",
                        "Münih",
                        "Hamburg",
                        "Frankfurt"
                    }
                },

                {
                    "İspanya",
                    new[]
                    {
                        "Madrid",
                        "Barselona",
                        "Valencia"
                    }
                }
            };

            var durations = new[]
            {
                "2 Gün 1 Gece",
                "3 Gün 2 Gece",
                "4 Gün 3 Gece",
                "5 Gün 4 Gece",
                "7 Gün 6 Gece"
            };

            var random = new Random();

            var routes = new List<Entities.Route>();

            for (var i = 0; i < count; i++)
            {
                var country =
                    countries.Keys
                        .ElementAt(
                            random.Next(countries.Count));

                var cities = countries[country];

                var city =
                    cities[random.Next(cities.Length)];

                var duration =
                    durations[random.Next(durations.Length)];

                var price =
                    random.Next(100, 3001);

                routes.Add(
                    new Entities.Route
                    {
                        Country = country,

                        City = city,

                        Duration = duration,

                        Price = price,

                        ImageUrl =
                            $"https://picsum.photos/seed/route{i}/600/400"
                    });
            }

            await _routeCollection.InsertManyAsync(routes);
        }
    }
}