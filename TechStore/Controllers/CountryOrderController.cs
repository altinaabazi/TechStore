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
    public class CountryOrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CountryOrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CountryOrders
        public async Task<IActionResult> Index()
        {
              return _context.CountryOrders != null ? 
                          View(await _context.CountryOrders.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.CountryOrders'  is null.");
        }

        // GET: CountryOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CountryOrders == null)
            {
                return NotFound();
            }

            var countryOrder = await _context.CountryOrders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (countryOrder == null)
            {
                return NotFound();
            }

            return View(countryOrder);
        }

        // GET: CountryOrders/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CountryOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] CountryOrder countryOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(countryOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(countryOrder);
        }

        // GET: CountryOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CountryOrders == null)
            {
                return NotFound();
            }

            var countryOrder = await _context.CountryOrders.FindAsync(id);
            if (countryOrder == null)
            {
                return NotFound();
            }
            return View(countryOrder);
        }

        // POST: CountryOrders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] CountryOrder countryOrder)
        {
            if (id != countryOrder.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(countryOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryOrderExists(countryOrder.Id))
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
            return View(countryOrder);
        }

        // GET: CountryOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CountryOrders == null)
            {
                return NotFound();
            }

            var countryOrder = await _context.CountryOrders
                .FirstOrDefaultAsync(m => m.Id == id);
            if (countryOrder == null)
            {
                return NotFound();
            }

            return View(countryOrder);
        }

        // POST: CountryOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CountryOrders == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CountryOrders'  is null.");
            }
            var countryOrder = await _context.CountryOrders.FindAsync(id);
            if (countryOrder != null)
            {
                _context.CountryOrders.Remove(countryOrder);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountryOrderExists(int id)
        {
          return (_context.CountryOrders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
