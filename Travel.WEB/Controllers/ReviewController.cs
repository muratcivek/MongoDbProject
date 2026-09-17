using Microsoft.AspNetCore.Mvc;
using Travel.WEB.DTOs.ReviewDTOs;
using Travel.WEB.Services.Review;

namespace Travel.WEB.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(
            IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateReviewDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.RouteId))
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                TempData["ReviewError"] =
                    "Yorum gönderilemedi. Alanları kontrol edin.";

                return RedirectToAction(
                    "Detail",
                    "Tour",
                    new
                    {
                        id = dto.RouteId
                    });
            }

            await _reviewService.CreateAsync(dto);

            TempData["ReviewSuccess"] =
                "Yorumunuz başarıyla gönderildi.";

            return RedirectToAction(
                "Detail",
                "Tour",
                new
                {
                    id = dto.RouteId
                });
        }
    }
}