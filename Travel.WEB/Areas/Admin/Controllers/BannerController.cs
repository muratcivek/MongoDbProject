using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Travel.WEB.DTOs.BannerDTOs;
using Travel.WEB.Services.Banner;

namespace Travel.WEB.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class BannerController(IBannerService _bannerService, IMapper _IMapper) : Controller
    {
        public async Task<IActionResult> Index()
        {
            var banners = await _bannerService.GetAllAsync();
            return View(banners);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBannerDto createBannerDto)
        {
            if(!ModelState.IsValid)
            {
                return View(createBannerDto);
            }
            await _bannerService.CreateAsync(createBannerDto);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(string id)
        {
            var banner = await _bannerService.GetByIdAsync(id);
            if (banner == null)
            {
                return NotFound();
            }
            var updateBannerDto = _IMapper.Map<UpdateBannerDto>(banner);
            return View(updateBannerDto);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateBannerDto updateBannerDto)
        {
             if(!ModelState.IsValid)
            {
                return View();
            }

            await _bannerService.UpdateAsync(updateBannerDto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
        
            await _bannerService.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
