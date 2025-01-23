using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechStore.Data;
using TechStore.Models;

namespace TechStore.Controllers
{
    public class BrandController : Controller
    {
        private readonly ApplicationDbContext _brandRepo;

        public BrandController(ApplicationDbContext _brandRepo)
        {
            _brandRepo = _brandRepo;
        }

        // GET: Brand
        public async Task<IActionResult> Index()
        {
            return _brandRepo.Brands != null ?
                        View(await _brandRepo.Brands.ToListAsync()) :
                        Problem("Entity set 'ApplicationDb_brandRepo.Brands'  is null.");
        }

        // GET: Brand/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _brandRepo.Brands == null)
            {
                return NotFound();
            }

            var brand = await _brandRepo.Brands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // GET: Brand/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Brand/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                _brandRepo.Add(brand);
                await _brandRepo.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        // GET: Brand/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _brandRepo.Brands == null)
            {
                return NotFound();
            }

            var brand = await _brandRepo.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // POST: Brand/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Brand brand)
        {
            if (id != brand.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _brandRepo.Update(brand);
                    await _brandRepo.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(brand.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        // GET: Brand/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _brandRepo.Brands == null)
            {
                return NotFound();
            }

            var brand = await _brandRepo.Brands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // POST: Brand/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_brandRepo.Brands == null)
            {
                return Problem("Entity set 'ApplicationDb_brandRepo.Brands'  is null.");
            }
            var brand = await _brandRepo.Brands.FindAsync(id);
            if (brand != null)
            {
                _brandRepo.Brands.Remove(brand);
            }

            await _brandRepo.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrandExists(int id)
        {
            return (_brandRepo.Brands?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
