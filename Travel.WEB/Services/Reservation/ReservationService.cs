using AutoMapper;
using MongoDB.Bson;
using MongoDB.Driver;
using Travel.WEB.DTOs.ReservationDTOs;
using Travel.WEB.Settings;

namespace Travel.WEB.Services.Reservation
{
    public class ReservationService : IReservationService
    {
        private readonly
            IMongoCollection<Entities.Reservation>
            _reservationCollection;
        private readonly string _routeCollectionName;
        private readonly IMapper _mapper;

        public ReservationService(
            IDatabaseSettings databaseSettings,
            IMapper mapper)
        {
            var client =
                new MongoClient(
                    databaseSettings.ConnectionString);

            var database =
                client.GetDatabase(
                    databaseSettings.DatabaseName);

            _reservationCollection =
                database.GetCollection<Entities.Reservation>(
                    databaseSettings.ReservationCollectionName);

            _routeCollectionName =
    databaseSettings.RouteCollectionName;

            _mapper = mapper;
        }

        public async Task<ResultReservationDto> CreateAsync(
            CreateReservationDto createReservationDto)
        {
            var reservation =
                _mapper.Map<Entities.Reservation>(
                    createReservationDto);

            reservation.TravelDate =
                DateTime.SpecifyKind(
                    createReservationDto.TravelDate.Date,
                    DateTimeKind.Utc);

            reservation.CreatedDate =
                DateTime.UtcNow;

            reservation.Status =
                "Pending";

            reservation.ReservationCode =
                GenerateReservationCode();

            await _reservationCollection
                .InsertOneAsync(reservation);

            return _mapper.Map<ResultReservationDto>(
                reservation);
        }

        private static string GenerateReservationCode()
        {
            var randomPart =
                Guid.NewGuid()
                    .ToString("N")
                    .Substring(0, 8)
                    .ToUpperInvariant();

            return $"TRV-{randomPart}";
        }


        public async Task<List<ResultReservationDto>>
            GetAllAsync()
        {
            var reservations =
                await _reservationCollection
                    .Find(_ => true)
                    .SortByDescending(
                        x => x.CreatedDate)
                    .ToListAsync();

            return _mapper.Map<
                List<ResultReservationDto>>(
                    reservations);
        }


        public async Task<List<ResultReservationDto>>
            GetByRouteIdAsync(string routeId)
        {
            var reservations =
                await _reservationCollection
                    .Find(
                        x => x.RouteId == routeId)
                    .SortByDescending(
                        x => x.CreatedDate)
                    .ToListAsync();

            return _mapper.Map<
                List<ResultReservationDto>>(
                    reservations);
        }


        public async Task<ResultReservationDto?>
            GetByIdAsync(string id)
        {
            var reservation =
                await _reservationCollection
                    .Find(x => x.Id == id)
                    .FirstOrDefaultAsync();

            if (reservation == null)
            {
                return null;
            }

            return _mapper.Map<
                ResultReservationDto>(
                    reservation);
        }


        public async Task UpdateStatusAsync(
            UpdateReservationStatusDto dto)
        {
            var filter =
                Builders<Entities.Reservation>
                    .Filter
                    .Eq(x => x.Id, dto.Id);

            var update =
                Builders<Entities.Reservation>
                    .Update
                    .Set(
                        x => x.Status,
                        dto.Status);

            await _reservationCollection
                .UpdateOneAsync(
                    filter,
                    update);
        }


        public async Task DeleteAsync(string id)
        {
            await _reservationCollection
                .DeleteOneAsync(
                    x => x.Id == id);
        }


        public async Task CreateIndexesAsync()
        {
            // RouteId index
            var routeIndex =
                new CreateIndexModel<Entities.Reservation>(
                    Builders<Entities.Reservation>
                        .IndexKeys
                        .Ascending(x => x.RouteId),
                    new CreateIndexOptions
                    {
                        Name = "idx_reservation_routeId"
                    });


            // CreatedDate index
            var dateIndex =
                new CreateIndexModel<Entities.Reservation>(
                    Builders<Entities.Reservation>
                        .IndexKeys
                        .Descending(x => x.CreatedDate),
                    new CreateIndexOptions
                    {
                        Name = "idx_reservation_createdDate"
                    });


            // ReservationCode UNIQUE index
            var reservationCodeIndex =
                new CreateIndexModel<Entities.Reservation>(
                    Builders<Entities.Reservation>
                        .IndexKeys
                        .Ascending(x => x.ReservationCode),
                    new CreateIndexOptions
                    {
                        Name = "idx_reservation_code",
                        Unique = true
                    });


            // Yukarıdaki indexlerin hepsini MongoDB'de oluştur
            await _reservationCollection
                .Indexes
                .CreateManyAsync(
                    new[]
                    {
                routeIndex,
                dateIndex,
                reservationCodeIndex
                    });
        }

        public async Task<ResultReservationDto?>
    GetByCodeAndEmailAsync(
        string reservationCode,
        string email)
        {
            var filter =
                Builders<Entities.Reservation>
                    .Filter
                    .And(
                        Builders<Entities.Reservation>
                            .Filter
                            .Eq(
                                x => x.ReservationCode,
                                reservationCode),

                        Builders<Entities.Reservation>
                            .Filter
                            .Eq(
                                x => x.Email,
                                email)
                    );

            var reservation =
                await _reservationCollection
                    .Find(filter)
                    .FirstOrDefaultAsync();

            if (reservation == null)
            {
                return null;
            }

            return _mapper.Map<ResultReservationDto>(
                reservation);
        }

        public async Task<List<ReservationCalendarItemDto>>
    GetCalendarAsync(
        DateTime startDate,
        DateTime endDate)
        {
            var pipeline = new[]
            {
        // 1) İstenen tarih aralığını al
        // Cancelled rezervasyonları takvimden çıkar
        new BsonDocument("$match",
            new BsonDocument
            {
                {
                    "TravelDate",
                    new BsonDocument
                    {
                        { "$gte", startDate },
                        { "$lt", endDate }
                    }
                },

                {
                    "Status",
                    new BsonDocument(
                        "$ne",
                        "Cancelled")
                }
            }),


        // 2) Tarih + RouteId bazında grupla
        new BsonDocument("$group",
            new BsonDocument
            {
                {
                    "_id",
                    new BsonDocument
                    {
                        { "TravelDate", "$TravelDate" },
                        { "RouteId", "$RouteId" }
                    }
                },

                {
                    "ReservationCount",
                    new BsonDocument(
                        "$sum",
                        1)
                },

                {
                    "PersonCount",
                    new BsonDocument(
                        "$sum",
                        "$PersonCount")
                }
            }),


        // RouteId string olduğu için
        // ObjectId'ye çeviriyoruz.
        new BsonDocument("$set",
            new BsonDocument(
                "RouteObjectId",
                new BsonDocument(
                    "$convert",
                    new BsonDocument
                    {
                        {
                            "input",
                            "$_id.RouteId"
                        },
                        {
                            "to",
                            "objectId"
                        },
                        {
                            "onError",
                            BsonNull.Value
                        },
                        {
                            "onNull",
                            BsonNull.Value
                        }
                    }
                )
            )),


        // 3) Routes collection ile JOIN benzeri işlem
        new BsonDocument("$lookup",
            new BsonDocument
            {
                {
                    "from",
                   _routeCollectionName
                },
                {
                    "localField",
                    "RouteObjectId"
                },
                {
                    "foreignField",
                    "_id"
                },
                {
                    "as",
                    "Route"
                }
            }),


        // Route array olarak gelir.
        // Tek route'a dönüştürüyoruz.
        new BsonDocument(
            "$unwind",
            "$Route"),


        // 4) İstediğimiz alanları çıkar
        new BsonDocument("$project",
            new BsonDocument
            {
                { "_id", 0 },

                {
                    "TravelDate",
                    "$_id.TravelDate"
                },

                {
                    "RouteId",
                    "$_id.RouteId"
                },

                {
                    "City",
                    "$Route.City"
                },

                {
                    "Country",
                    "$Route.Country"
                },

                {
                    "ReservationCount",
                    1
                },

                {
                    "PersonCount",
                    1
                }
            }),


        // Tarihe göre sırala
        new BsonDocument(
            "$sort",
            new BsonDocument(
                "TravelDate",
                1))
    };


            return await _reservationCollection
                .Aggregate<ReservationCalendarItemDto>(
                    pipeline)
                .ToListAsync();
        }


        public async Task<List<ResultReservationDto>>
         GetByTravelDateAsync(DateTime date)
        {
            var startDate =
                DateTime.SpecifyKind(
                    date.Date,
                    DateTimeKind.Utc);

            var endDate =
                startDate.AddDays(1);

            var filter =
                Builders<Entities.Reservation>
                    .Filter
                    .And(
                        Builders<Entities.Reservation>
                            .Filter
                            .Gte(
                                x => x.TravelDate,
                                startDate),

                        Builders<Entities.Reservation>
                            .Filter
                            .Lt(
                                x => x.TravelDate,
                                endDate),

                        Builders<Entities.Reservation>
                            .Filter
                            .Ne(
                                x => x.Status,
                                "Cancelled")
                    );

            var reservations =
                await _reservationCollection
                    .Find(filter)
                    .SortBy(x => x.RouteId)
                    .ToListAsync();

            return _mapper.Map<List<ResultReservationDto>>(
                reservations);
        }
    }
}