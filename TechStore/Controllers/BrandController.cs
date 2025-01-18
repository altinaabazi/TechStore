using Humanizer.Localisation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechStore.Constants;
using TechStore.Models;
using TechStore.Models.DTOs;
using TechStore.Repositories;
namespace TechStore.Controllers
{
    //[Authorize(Roles = nameof(Roles.Admin))]
    public class BrandController : Controller
    {
        private readonly IBrandRepository _brandRepo;

        public BrandController(IBrandRepository brandRepo)
        {
            _brandRepo = brandRepo;
        }

        public async Task<IActionResult> Index()
        {
            var brands = await _brandRepo.GetBrands();
            return View(brands);
        }

        public IActionResult AddBrand()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddBrand(BrandDTO brand)
        {
            if (!ModelState.IsValid)
            {
                return View(brand);
            }
            try
            {
                var brandToAdd = new Brand { Name = brand.BrandName, Id = brand.Id };
                await _brandRepo.AddBrand(brandToAdd);
                TempData["successMessage"] = "Brand added successfully";
                return RedirectToAction(nameof(AddBrand));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Genre could not added!";
                return View(brand);
            }

        }
        public async Task<IActionResult> UpdateBrand(int id)
        {
            var brand = await _brandRepo.GetCategoryById(id);
            if (brand is null)
                throw new InvalidOperationException($"Brand with id: {id} does not found");
            var brandToUpdate = new BrandDTO
            {
                Id = brand.Id,
                BrandName = brand.Name
            };
            return View(brandToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBrand(BrandDTO brandToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return View(brandToUpdate);
            }
            try
            {
                var brand = new Brand { Name = brandToUpdate.BrandName, Id = brandToUpdate.Id };
                await _brandRepo.UpdateBrand(brand);
                TempData["successMessage"] = "Brand is updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Brand could not updated!";
                return View(brandToUpdate);
            }

        }

        public async Task<IActionResult> DeleteBrand(int id)
        {
            var brand = await _brandRepo.GetCategoryById(id);
            if (brand is null)
                throw new InvalidOperationException($"Brand with id: {id} does not found");
            await _brandRepo.DeleteBrand(brand);
            return RedirectToAction(nameof(Index));
        }

    }
}