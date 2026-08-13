using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Travel.WEB.DTOs.BannerDTOs;
using Travel.WEB.DTOs.RouteDTOs;
using Travel.WEB.Services.Route;

namespace Travel.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RouteController(IRouteService _routeService, IMapper _mapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var routes = await _routeService.GetAllAsync();
            return View(routes);
        }

        [HttpPost]
        public async Task<IActionResult> SearchCity(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
            {
                var allRoutes = await _routeService.GetAllAsync();
                return View("Index", allRoutes);
            }

            var routes = await _routeService.GetAllByCityAsync(city);

            if (routes.Count == 0)
            {
                routes = await _routeService.GetAllAsync();
            }

            return View("Index", routes);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateRouteDto createRouteDto)
        {
            if (!ModelState.IsValid)
            {
                return View(createRouteDto);
            }
            await _routeService.CreateAsync(createRouteDto);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(string id)
        {
            var route = await _routeService.GetByIdAsync(id);
            if (route == null) return NotFound();
            var updateRouteDto = _mapper.Map<UpdateRouteDto>(route);
            return View(updateRouteDto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateRouteDto updateRouteDto)
        {
            if (!ModelState.IsValid)
            {
                return View(updateRouteDto);
            }
            await _routeService.UpdateAsync(updateRouteDto);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(string id)
        {

            await _routeService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

    }
}
