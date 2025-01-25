using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TechStore.Models;
using TechStore.Repositories;

namespace TechStore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _catoRepo;

        public CategoryController(ICategoryRepository catoRepo)
        {
            _catoRepo = catoRepo;
        }

        // GET: catogory
        public async Task<IActionResult> Index()
        {
            var catogory = await _catoRepo.GetCategories();
            return View(catogory);
        }

        // GET: catogory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catogory = await _catoRepo.GetCategoryById(id.Value);
            if (catogory == null)
            {
                return NotFound();
            }

            return View(catogory);
        }

        // GET: Catogory/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Catogory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Category catogory)
        {
            if (ModelState.IsValid)
            {
                await _catoRepo.AddCategory(catogory);
                return RedirectToAction(nameof(Index));
            }
            return View(catogory);
        }

        // GET: Catogory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catogory = await _catoRepo.GetCategoryById(id.Value);
            if (catogory == null)
            {
                return NotFound();
            }

            return View(catogory);
        }

        // POST: Catogory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Category catogory)
        {
            if (id != catogory.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _catoRepo.UpdateCategory(catogory);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating the catogory.");
                    return View(catogory);
                }
                return RedirectToAction(nameof(Index));
            }

            return View(catogory);
        }

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catogory = await _catoRepo.GetCategoryById(id.Value);
            if (catogory == null)
            {
                return NotFound();
            }

            return View(catogory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var catogory = await _catoRepo.GetCategoryById(id);
            if (catogory == null)
            {
                return NotFound();
            }

            await _catoRepo.DeleteCategory(catogory);
            return RedirectToAction(nameof(Index));
        }


    }
}
