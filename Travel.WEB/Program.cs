using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.Options;
using System.Reflection;
using Travel.WEB.Seeders;
using Travel.WEB.Services.Banner;
using Travel.WEB.Services.Reservation;
using Travel.WEB.Services.Review;
using Travel.WEB.Services.Route;
using Travel.WEB.Settings;
using static System.Formats.Asn1.AsnWriter;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));
builder.Services.AddSingleton<IDatabaseSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});
builder.Services.AddScoped<IBannerService, BannerService>();
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddScoped<
    IReviewService,
    ReviewService>();
builder.Services.AddScoped<
    IReservationService,
    ReservationService>();
builder.Services.AddScoped<RouteSeeder>();
builder.Services.AddScoped<DemoDataSeeder>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();


//if (app.Environment.IsDevelopment())
//{
//    using var scope = app.Services.CreateScope();

//    var routeSeeder =
//        scope.ServiceProvider
//            .GetRequiredService<RouteSeeder>();

//    await routeSeeder.SeedAsync(10000);
//}

if (app.Environment.IsDevelopment())
{
    using var scope =
        app.Services.CreateScope();

    var demoDataSeeder =
        scope.ServiceProvider
            .GetRequiredService<DemoDataSeeder>();

    await demoDataSeeder.SeedAsync();
}

using (var scope = app.Services.CreateScope())
{
    var routeService =
        scope.ServiceProvider
            .GetRequiredService<IRouteService>();

    var reviewService =
        scope.ServiceProvider
            .GetRequiredService<IReviewService>();

    var reservationService =
        scope.ServiceProvider
            .GetRequiredService<IReservationService>();

    await routeService.CreateIndexesAsync();

    await reviewService.CreateIndexesAsync();

    await reservationService.CreateIndexesAsync();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tour}/{action=Index}/{id?}");

app.Run();
