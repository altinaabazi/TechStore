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
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryController(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        public async Task<IActionResult> Index()
        {
            var category = await _categoryRepo.GetCategories();
            return View(category);
        }

        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryDTO category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            try
            {
                var categoryToAdd = new Category { Name = category.CategoryName, Id = category.Id };
                await _categoryRepo.AddCategory(categoryToAdd);
                TempData["successMessage"] = "Category added successfully";
                return RedirectToAction(nameof(AddCategory));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "Category could not added!";
                return View(category);
            }

        }
        public async Task<IActionResult> UpdateCategory(int id)
        {
            var category = await _categoryRepo.GetCategoryById(id);
            if (category is null)
                throw new InvalidOperationException($"Category with id: {id} does not found");
            var CategoryToUpdate = new CategoryDTO
            {
                Id = category.Id,
                CategoryName = category.Name
            };
            return View(CategoryToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(CategoryDTO CategoryToUpdate)
        {
            if (!ModelState.IsValid)
            {
                return View(CategoryToUpdate);
            }
            try
            {
                var category = new Category { Name = CategoryToUpdate.CategoryName, Id = CategoryToUpdate.Id };
                await _categoryRepo.UpdateCategory(category);
                TempData["successMessage"] = "category is updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = "category could not updated!";
                return View(CategoryToUpdate);
            }

        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _categoryRepo.GetCategoryById(id);
            if (category is null)
                throw new InvalidOperationException($"category with id: {id} does not found");
            await _categoryRepo.DeleteCategory(category);
            return RedirectToAction(nameof(Index));
        }

    }
}