using Microsoft.AspNetCore.Mvc;
using Travel.WEB.DTOs.ReservationDTOs;
using Travel.WEB.Models.Public;
using Travel.WEB.Services.Reservation;
using Travel.WEB.Services.Route;

namespace Travel.WEB.Controllers
{
    public class ReservationController : Controller
    {
        private readonly
            IReservationService _reservationService;

        private readonly
            IRouteService _routeService;

        public ReservationController(
            IReservationService reservationService,
            IRouteService routeService)
        {
            _reservationService =
                reservationService;

            _routeService =
                routeService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateReservationDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.RouteId))
            {
                return BadRequest();
            }

            var route =
                await _routeService.GetByIdAsync(dto.RouteId);

            if (route == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                TempData["ReservationError"] =
                    "Rezervasyon oluşturulamadı. Lütfen bilgileri kontrol edin.";

                return RedirectToAction(
                    "Detail",
                    "Tour",
                    new { id = dto.RouteId });
            }

            // Rezervasyonu oluşturuyoruz ve oluşan kaydı geri alıyoruz
            var reservation =
                await _reservationService.CreateAsync(dto);

            // Kullanıcıya rezervasyon kodunu gösteriyoruz
            TempData["ReservationSuccess"] =
                $"Rezervasyon talebiniz alındı. " +
                $"Rezervasyon kodunuz: {reservation.ReservationCode}";

            return RedirectToAction(
                "Detail",
                "Tour",
                new { id = dto.RouteId });
        }

        [HttpGet]
        public IActionResult Track()
        {
            return View(
                new ReservationTrackViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Track(
    ReservationTrackViewModel model)
        {
            model.Searched = true;

            if (string.IsNullOrWhiteSpace(
                    model.Search.ReservationCode) ||
                string.IsNullOrWhiteSpace(
                    model.Search.Email))
            {
                return View(model);
            }

            var reservation =
                await _reservationService
                    .GetByCodeAndEmailAsync(
                        model.Search.ReservationCode.Trim(),
                        model.Search.Email.Trim());

            if (reservation == null)
            {
                return View(model);
            }

            model.Reservation = reservation;

            model.Route =
                await _routeService
                    .GetByIdAsync(
                        reservation.RouteId);

            return View(model);
        }
    }
}