using MongoDB.Driver;
using Travel.WEB.Entities;
using Travel.WEB.Entities.Embedded;
using Travel.WEB.Settings;
using Route = Travel.WEB.Entities.Route;

namespace Travel.WEB.Seeders
{
    public class DemoDataSeeder
    {
        private readonly IMongoCollection<Route> _routeCollection;
        private readonly IMongoCollection<Review> _reviewCollection;
        private readonly IMongoCollection<Reservation> _reservationCollection;
        private readonly IMongoCollection<Banner> _bannerCollection;

        public DemoDataSeeder(IDatabaseSettings databaseSettings)
        {
            var client =
                new MongoClient(databaseSettings.ConnectionString);

            var database =
                client.GetDatabase(databaseSettings.DatabaseName);

            _routeCollection =
                database.GetCollection<Route>(
                    databaseSettings.RouteCollectionName);

            _reviewCollection =
                database.GetCollection<Review>(
                    databaseSettings.ReviewCollectionName);

            _reservationCollection =
                database.GetCollection<Reservation>(
                    databaseSettings.ReservationCollectionName);

            _bannerCollection =
                database.GetCollection<Banner>(
                    databaseSettings.BannerCollectionName);
        }

        public async Task SeedAsync()
        {
            // DEVELOPMENT DEMO DATA RESET
            await _reviewCollection.DeleteManyAsync(_ => true);
            await _reservationCollection.DeleteManyAsync(_ => true);
            await _bannerCollection.DeleteManyAsync(_ => true);
            await _routeCollection.DeleteManyAsync(_ => true);

            var routes = CreateRoutes();

            await _routeCollection.InsertManyAsync(routes);

            var reviews =
                CreateReviews(routes);

            await _reviewCollection.InsertManyAsync(reviews);

            var reservations =
                CreateReservations(routes);

            await _reservationCollection.InsertManyAsync(
                reservations);

            var banners =
                CreateBanners();

            await _bannerCollection.InsertManyAsync(banners);
        }

        private static List<Route> CreateRoutes()
        {
            return new List<Route>
            {
                new()
                {
                    City = "İstanbul",
                    Country = "Türkiye",
                    Duration = "3 Gün 2 Gece",
                    Price = 1850,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1524231757912-21f4fe3a7200?auto=format&fit=crop&w=1200&q=80",
                    Features = new()
                    {
                        "Kahvaltı",
                        "Rehber",
                        "Boğaz Turu"
                    },
                    Details = new RouteDetails
                    {
                        Transportation = "Otobüs",
                        Accommodation = "4 Yıldızlı Otel",
                        Description =
                            "İstanbul'un tarihi yarımadasını, Boğaz hattını ve önemli kültürel noktalarını kapsayan şehir turu."
                    }
                },

                new()
                {
                    City = "Eskişehir",
                    Country = "Türkiye",
                    Duration = "2 Gün 1 Gece",
                    Price = 950,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1596394516093-501ba68a0ba6?auto=format&fit=crop&w=1200&q=80",
                    Features = new()
                    {
                        "Porsuk Turu",
                        "Kahvaltı",
                        "Şehir Rehberi"
                    },
                    Details = new RouteDetails
                    {
                        Transportation = "Hızlı Tren",
                        Accommodation = "Butik Otel",
                        Description =
                            "Porsuk Çayı, Odunpazarı ve şehir merkezini kapsayan keyifli Eskişehir keşif turu."
                    }
                },

                new()
                {
                    City = "Kapadokya",
                    Country = "Türkiye",
                    Duration = "3 Gün 2 Gece",
                    Price = 2750,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1528181304800-259b08848526?auto=format&fit=crop&w=1200&q=80",
                    Features = new()
                    {
                        "Balon Turu",
                        "Kahvaltı",
                        "Transfer"
                    },
                    Details = new RouteDetails
                    {
                        Transportation = "Otobüs",
                        Accommodation = "Taş Otel",
                        Description =
                            "Peribacaları, vadiler ve gün doğumu balon manzaralarıyla Kapadokya'nın öne çıkan noktalarını keşfedin."
                    }
                },

                new()
                {
                    City = "Antalya",
                    Country = "Türkiye",
                    Duration = "4 Gün 3 Gece",
                    Price = 3200,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1529107386315-e1a2ed48a620?auto=format&fit=crop&w=1200&q=80",
                    Features = new()
                    {
                        "Deniz",
                        "Kahvaltı",
                        "Havalimanı Transferi"
                    },
                    Details = new RouteDetails
                    {
                        Transportation = "Uçak",
                        Accommodation = "5 Yıldızlı Otel",
                        Description =
                            "Akdeniz kıyıları, Kaleiçi ve çevre rotalarıyla dinlenme ve keşfi bir araya getiren Antalya turu."
                    }
                },

                new()
                {
                    City = "İzmir",
                    Country = "Türkiye",
                    Duration = "3 Gün 2 Gece",
                    Price = 2100,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1569959220744-ff553533f492?auto=format&fit=crop&w=1200&q=80",
                    Features = new()
                    {
                        "Kordon",
                        "Kahvaltı",
                        "Çeşme Turu"
                    },
                    Details = new RouteDetails
                    {
                        Transportation = "Otobüs",
                        Accommodation = "4 Yıldızlı Otel",
                        Description =
                            "İzmir merkezi, Kordon ve Çeşme çevresini kapsayan Ege rotası."
                    }
                },

                new()
                {
                    City = "Bursa",
                    Country = "Türkiye",
                    Duration = "2 Gün 1 Gece",
                    Price = 1350,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1602002418816-5c0aeef426aa?auto=format&fit=crop&w=1200&q=80",
                    Features = new()
                    {
                        "Uludağ",
                        "Kahvaltı",
                        "Rehber"
                    },
                    Details = new RouteDetails
                    {
                        Transportation = "Otobüs",
                        Accommodation = "Şehir Oteli",
                        Description =
                            "Tarihi Bursa merkezi ve Uludağ çevresini bir araya getiren hafta sonu turu."
                    }
                },

                new()
                {
                    City = "Trabzon",
                    Country = "Türkiye",
                    Duration = "4 Gün 3 Gece",
                    Price = 2950,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1500530855697-b586d89ba3ee?auto=format&fit=crop&w=1200&q=80",
                    Features = new()
                    {
                        "Uzungöl",
                        "Yayla Turu",
                        "Kahvaltı"
                    },
                    Details = new RouteDetails
                    {
                        Transportation = "Uçak",
                        Accommodation = "Yayla Oteli",
                        Description =
                            "Doğa, yayla ve Karadeniz manzaraları odaklı Trabzon ve çevresi turu."
                    }
                },

                new()
                {
                    City = "Mardin",
                    Country = "Türkiye",
                    Duration = "3 Gün 2 Gece",
                    Price = 2450,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1518005020951-eccb494ad742?auto=format&fit=crop&w=1200&q=80",
                    Features = new()
                    {
                        "Rehber",
                        "Kahvaltı",
                        "Midyat"
                    },
                    Details = new RouteDetails
                    {
                        Transportation = "Uçak",
                        Accommodation = "Taş Konak",
                        Description =
                            "Mardin'in taş sokakları, tarihi yapıları ve Midyat çevresini kapsayan kültür turu."
                    }
                },

                new()
                {
                    City = "Fethiye",
                    Country = "Türkiye",
                    Duration = "4 Gün 3 Gece",
                    Price = 3400,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1533104816931-20fa691ff6ca?auto=format&fit=crop&w=1200&q=80",
                    Features = new()
                    {
                        "Tekne Turu",
                        "Kahvaltı",
                        "Ölüdeniz"
                    },
                    Details = new RouteDetails
                    {
                        Transportation = "Otobüs",
                        Accommodation = "Sahil Oteli",
                        Description =
                            "Ölüdeniz, koylar ve tekne turuyla doğa ağırlıklı Fethiye tatili."
                    }
                },

                new()
                {
                    City = "Çanakkale",
                    Country = "Türkiye",
                    Duration = "2 Gün 1 Gece",
                    Price = 1450,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1500534314209-a25ddb2bd429?auto=format&fit=crop&w=1200&q=80",
                    Features = new()
                    {
                        "Troya",
                        "Rehber",
                        "Kahvaltı"
                    },
                    Details = new RouteDetails
                    {
                        Transportation = "Otobüs",
                        Accommodation = "Şehir Oteli",
                        Description =
                            "Çanakkale merkezi, Troya ve tarihi alanları kapsayan kültür rotası."
                    }
                },

                new()
                {
                    City = "Safranbolu",
                    Country = "Türkiye",
                    Duration = "2 Gün 1 Gece",
                    Price = 1250,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1470214304380-aadaedcfff1b?auto=format&fit=crop&w=1200&q=80",
                    Features = new()
                    {
                        "Konaklama",
                        "Kahvaltı",
                        "Rehber"
                    },
                    Details = new RouteDetails
                    {
                        Transportation = "Otobüs",
                        Accommodation = "Tarihi Konak",
                        Description =
                            "Safranbolu'nun tarihi konakları ve geleneksel sokak dokusunu keşfetmeye yönelik kısa tur."
                    }
                },

                new()
                {
                    City = "Şanlıurfa",
                    Country = "Türkiye",
                    Duration = "3 Gün 2 Gece",
                    Price = 2300,
                    ImageUrl =
                        "https://images.unsplash.com/photo-1548013146-72479768bada?auto=format&fit=crop&w=1200&q=80",
                    Features = new()
                    {
                        "Göbeklitepe",
                        "Rehber",
                        "Kahvaltı"
                    },
                    Details = new RouteDetails
                    {
                        Transportation = "Uçak",
                        Accommodation = "4 Yıldızlı Otel",
                        Description =
                            "Göbeklitepe, Balıklıgöl ve Şanlıurfa'nın kültürel noktalarını kapsayan keşif turu."
                    }
                }
            };
        }

        private static List<Review> CreateReviews(
            List<Route> routes)
        {
            var comments = new[]
            {
                "Program gayet düzenliydi, özellikle rehber anlatımları çok başarılıydı.",
                "Konaklama ve ulaşım beklentimizi karşıladı. Tekrar tercih edebilirim.",
                "Tur planı dengeliydi, yeterince serbest zaman da vardı.",
                "İlk kez katıldım ve genel olarak oldukça memnun kaldım.",
                "Özellikle şehir turu ve organizasyon tarafı çok başarılıydı.",
                "Fiyatına göre oldukça iyi bir deneyimdi.",
                "Program yoğun değildi, rahat ve keyifli geçti.",
                "Rehberin bölge hakkında verdiği bilgiler turu çok daha güzel hale getirdi."
            };

            var names = new[]
            {
                "Ayşe Demir",
                "Mehmet Kaya",
                "Elif Yılmaz",
                "Burak Şahin",
                "Zeynep Arslan",
                "Emre Aydın",
                "Selin Koç",
                "Can Özdemir"
            };

            var reviews = new List<Review>();

            var random = new Random(42);

            foreach (var route in routes)
            {
                var reviewCount =
                    random.Next(2, 5);

                for (var i = 0;
                     i < reviewCount;
                     i++)
                {
                    reviews.Add(
                        new Review
                        {
                            RouteId = route.Id,
                            UserName =
                                names[random.Next(names.Length)],
                            Rating =
                                random.Next(4, 6),
                            Comment =
                                comments[random.Next(comments.Length)],
                            CreatedDate =
                                DateTime.UtcNow
                                    .AddDays(
                                        -random.Next(1, 40))
                        });
                }
            }

            return reviews;
        }

        private static List<Reservation>
            CreateReservations(
                List<Route> routes)
        {
            var names = new[]
            {
                "Mert Kaya",
                "Ayşe Yılmaz",
                "Burak Demir",
                "Elif Arslan",
                "Can Aydın",
                "Selin Şahin",
                "Ece Koç",
                "Kerem Özdemir"
            };

            var statuses = new[]
            {
                "Approved",
                "Approved",
                "Pending",
                "Pending",
                "Cancelled"
            };

            var travelDates = new[]
            {
                new DateTime(2026, 9, 21, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2026, 9, 23, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2026, 9, 25, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2026, 9, 28, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2026, 9, 30, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2026, 10, 2, 0, 0, 0, DateTimeKind.Utc),
                new DateTime(2026, 10, 5, 0, 0, 0, DateTimeKind.Utc)
            };

            var reservations =
                new List<Reservation>();

            var random =
                new Random(81);

            for (var i = 0;
                 i < 28;
                 i++)
            {
                var route =
                    routes[random.Next(routes.Count)];

                var fullName =
                    names[random.Next(names.Length)];

                reservations.Add(
                    new Reservation
                    {
                        RouteId = route.Id,

                        ReservationCode =
                            $"TRV-DEMO-{(i + 1):000}",

                        FullName = fullName,

                        Email =
                            $"demo{i + 1}@travel.test",

                        Phone =
                            $"05{random.Next(30, 56)}" +
                            $"{random.Next(1000000, 9999999)}",

                        PersonCount =
                            random.Next(1, 5),

                        TravelDate =
                            travelDates[
                                random.Next(
                                    travelDates.Length)],

                        CreatedDate =
                            DateTime.UtcNow
                                .AddDays(
                                    -random.Next(1, 25)),

                        Status =
                            statuses[
                                random.Next(statuses.Length)]
                    });
            }

            return reservations;
        }

        private static List<Banner> CreateBanners()
        {
            return new List<Banner>
            {
                new()
                {
                    Title =
                        "Yeni Rotalar Seni Bekliyor",
                    Description =
                        "Türkiye'nin en sevilen rotalarını keşfet ve bir sonraki yolculuğunu planla.",
                    ImageUrl =
                        "https://images.unsplash.com/photo-1500530855697-b586d89ba3ee?auto=format&fit=crop&w=1600&q=80"
                },

                new()
                {
                    Title =
                        "Hafta Sonu Kaçamakları",
                    Description =
                        "Kısa süreli ama dolu dolu rotalarla hafta sonunu unutulmaz hale getir.",
                    ImageUrl =
                        "https://images.unsplash.com/photo-1464822759023-fed622ff2c3b?auto=format&fit=crop&w=1600&q=80"
                },

                new()
                {
                    Title =
                        "Kültür ve Keşif",
                    Description =
                        "Tarihi şehirler, yerel deneyimler ve rehberli rotalarla farklı hikâyeler keşfet.",
                    ImageUrl =
                        "https://images.unsplash.com/photo-1526778548025-fa2f459cd5c1?auto=format&fit=crop&w=1600&q=80"
                }
            };
        }
    }
}