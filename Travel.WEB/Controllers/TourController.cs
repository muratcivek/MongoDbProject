using Microsoft.AspNetCore.Mvc;
using Travel.WEB.DTOs.ReservationDTOs;
using Travel.WEB.DTOs.ReviewDTOs;
using Travel.WEB.DTOs.RouteDTOs;
using Travel.WEB.Models.Public;
using Travel.WEB.Services.Review;
using Travel.WEB.Services.Route;

namespace Travel.WEB.Controllers
{
    public class TourController : Controller
    {
        private readonly IRouteService _routeService;
        private readonly IReviewService _reviewService;

        public TourController(
            IRouteService routeService,
            IReviewService reviewService)
        {
            _routeService = routeService;
            _reviewService = reviewService;
        }


        public async Task<IActionResult> Index(
       [FromQuery] RouteFilterDto filter)
        {
            if (!Request.Query.ContainsKey("PageSize"))
            {
                filter.PageSize = 9;
            }

            var routes =
                await _routeService.GetFilteredAsync(filter);

            return View(routes);
        }


        public async Task<IActionResult> Detail(
            string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }


            var route =
                await _routeService
                    .GetByIdAsync(id);

            if (route == null)
            {
                return NotFound();
            }


            var reviews =
                await _reviewService
                    .GetByRouteIdAsync(id);


            var viewModel =
                new TourDetailViewModel
                {
                    Route = route,

                    Reviews = reviews,

                    ReviewForm =
                        new CreateReviewDto
                        {
                            RouteId = id
                        },

                    ReservationForm =
                        new CreateReservationDto
                        {
                            RouteId = id,

                            PersonCount = 1,

                            TravelDate =
                                DateTime.Today.AddDays(1)
                        }
                };


            return View(viewModel);
        }
    }
}