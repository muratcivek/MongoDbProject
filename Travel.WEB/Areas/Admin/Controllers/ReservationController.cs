using Microsoft.AspNetCore.Mvc;
using Travel.WEB.DTOs.ReservationDTOs;
using Travel.WEB.Models.Admin;
using Travel.WEB.Services.Reservation;
using Travel.WEB.Services.Route;

namespace Travel.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IRouteService _routeService;

        public ReservationController(
            IReservationService reservationService,
            IRouteService routeService)
        {
            _reservationService = reservationService;
            _routeService = routeService;
        }


        public async Task<IActionResult> Index()
        {
            var reservations =
                await _reservationService.GetAllAsync();

            var model =
                new List<ReservationListItemViewModel>();


            foreach (var reservation in reservations)
            {
                var route =
                    await _routeService.GetByIdAsync(
                        reservation.RouteId);

                model.Add(
                    new ReservationListItemViewModel
                    {
                        Reservation = reservation,
                        Route = route
                    });
            }


            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(
            string id,
            string status)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }


            var allowedStatuses = new[]
            {
                "Pending",
                "Approved",
                "Cancelled"
            };


            if (!allowedStatuses.Contains(status))
            {
                return BadRequest();
            }


            var reservation =
                await _reservationService.GetByIdAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }


            await _reservationService.UpdateStatusAsync(
                new UpdateReservationStatusDto
                {
                    Id = id,
                    Status = status
                });


            TempData["ReservationSuccess"] =
                "Rezervasyon durumu güncellendi.";


            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }


            var reservation =
                await _reservationService.GetByIdAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }


            await _reservationService.DeleteAsync(id);


            TempData["ReservationSuccess"] =
                "Rezervasyon silindi.";


            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Calendar(
    int? year,
    int? month)
        {
            var today =
                DateTime.Today;

            var selectedYear =
                year ?? today.Year;

            var selectedMonth =
                month ?? today.Month;


            if (selectedMonth < 1 ||
                selectedMonth > 12)
            {
                return BadRequest();
            }


            var startDate =
                new DateTime(
                    selectedYear,
                    selectedMonth,
                    1);

            var endDate =
                startDate.AddMonths(1);


            var items =
                await _reservationService
                    .GetCalendarAsync(
                        startDate,
                        endDate);


            var viewModel =
                new ReservationCalendarViewModel
                {
                    Year = selectedYear,
                    Month = selectedMonth,
                    Items = items
                };


            return View(viewModel);
        }

        public async Task<IActionResult> Day(
    DateTime date)
        {
            var reservations =
                await _reservationService
                    .GetByTravelDateAsync(date);


            var items =
                new List<ReservationDayItemViewModel>();


            foreach (var reservation in reservations)
            {
                var route =
                    await _routeService
                        .GetByIdAsync(
                            reservation.RouteId);


                items.Add(
                    new ReservationDayItemViewModel
                    {
                        Reservation =
                            reservation,

                        Route =
                            route
                    });
            }


            var viewModel =
                new ReservationDayViewModel
                {
                    Date = date,
                    Items = items
                };


            return View(viewModel);
        }
    }
}