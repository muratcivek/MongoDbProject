using Microsoft.AspNetCore.Mvc;
using Travel.WEB.Services.Review;

namespace Travel.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        public async Task<IActionResult> Index()
        {
            var reviews =
                await _reviewService.GetAllAsync();

            return View(reviews);
        }

        public async Task<IActionResult> RouteReviews(
            string routeId)
        {
            if (string.IsNullOrWhiteSpace(routeId))
            {
                return BadRequest();
            }

            var reviews =
                await _reviewService
                    .GetByRouteIdAsync(routeId);

            return View(reviews);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(
      string id,
      string? routeId,
      bool returnToList = false)
        {
            await _reviewService.DeleteAsync(id);

            TempData["ReviewSuccess"] =
                "Yorum başarıyla silindi.";

            if (returnToList)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(
                "Update",
                "Route",
                new
                {
                    area = "Admin",
                    id = routeId
                });
        }
    }
}