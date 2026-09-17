using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Travel.WEB.DTOs.ReviewDTOs;
using Travel.WEB.DTOs.RouteDTOs;
using Travel.WEB.Models.Admin;
using Travel.WEB.Services.Review;
using Travel.WEB.Services.Route;

namespace Travel.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RouteController : Controller
    {
        private readonly IRouteService _routeService;
        private readonly IReviewService _reviewService;
        private readonly IMapper _mapper;

        public RouteController(
            IRouteService routeService,
            IReviewService reviewService,
            IMapper mapper)
        {
            _routeService = routeService;
            _reviewService = reviewService;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index(
            [FromQuery] RouteFilterDto filter)
        {
            var routes =
                await _routeService.GetFilteredAsync(filter);

            var viewModel = new RouteIndexViewModel
            {
                Filter = filter,
                Routes = routes
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View(new CreateRouteDto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateRouteDto createRouteDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createRouteDto);
            }

            await _routeService.CreateAsync(createRouteDto);

            TempData["RouteSuccess"] =
                "Rota başarıyla oluşturuldu.";

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var route =
                await _routeService.GetByIdAsync(id);

            if (route == null)
            {
                return NotFound();
            }

            var routeDto =
                _mapper.Map<UpdateRouteDto>(route);

            var reviews =
                await _reviewService.GetByRouteIdAsync(id);

            var viewModel = new RouteUpdateViewModel
            {
                Route = routeDto,
                Reviews = reviews
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(
      RouteUpdateViewModel viewModel)
        {
            if (viewModel.Route == null ||
                string.IsNullOrWhiteSpace(viewModel.Route.Id))
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                viewModel.Reviews =
                    await _reviewService.GetByRouteIdAsync(
                        viewModel.Route.Id);

                return View(viewModel);
            }

            await _routeService.UpdateAsync(
                viewModel.Route);

            TempData["RouteSuccess"] =
                "Rota başarıyla güncellendi.";

            return RedirectToAction(
                nameof(Update),
                new
                {
                    id = viewModel.Route.Id
                });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            var route =
                await _routeService.GetByIdAsync(id);

            if (route == null)
            {
                return NotFound();
            }

            await _routeService.DeleteAsync(id);

            TempData["RouteSuccess"] =
                "Rota başarıyla silindi.";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFeature(
            string id,
            string feature)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(feature))
            {
                TempData["RouteError"] =
                    "Özellik boş bırakılamaz.";

                return RedirectToAction(
                    nameof(Update),
                    new { id });
            }

            var route =
                await _routeService.GetByIdAsync(id);

            if (route == null)
            {
                return NotFound();
            }

            await _routeService.AddFeatureAsync(
                id,
                feature.Trim());

            TempData["RouteSuccess"] =
                "Özellik başarıyla eklendi.";

            return RedirectToAction(
                nameof(Update),
                new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFeature(
            string id,
            string feature)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(feature))
            {
                return BadRequest();
            }

            var route =
                await _routeService.GetByIdAsync(id);

            if (route == null)
            {
                return NotFound();
            }

            await _routeService.RemoveFeatureAsync(
                id,
                feature);

            TempData["RouteSuccess"] =
                "Özellik kaldırıldı.";

            return RedirectToAction(
                nameof(Update),
                new { id });
        }
    }
}