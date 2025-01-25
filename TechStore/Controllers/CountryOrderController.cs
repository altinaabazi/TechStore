using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechStore.Data;
using TechStore.Models;
using TechStore.Repositories;

namespace TechStore.Controllers
{
    public class CountryOrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IAuditLogRepository _auditLogRepo;

        public CountryOrderController(ApplicationDbContext context, IAuditLogRepository auditLogRepo)
        {
            _context = context;
            _auditLogRepo = auditLogRepo;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] CountryOrder countryOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(countryOrder);
                await _context.SaveChangesAsync();

                // Audit Log
                var auditLog = new AuditLog
                {
                    Action = "Created",
                    Entity = "CountryOrder",
                    EntityId = countryOrder.Id,
                    PerformedBy = User.Identity.Name,
                    PerformedAt = DateTime.UtcNow
                };
                await _auditLogRepo.AddAuditLog(auditLog);

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

                    // Audit Log
                    var auditLog = new AuditLog
                    {
                        Action = "Updated",
                        Entity = "CountryOrder",
                        EntityId = countryOrder.Id,
                        PerformedBy = User.Identity.Name,
                        PerformedAt = DateTime.UtcNow
                    };
                    await _auditLogRepo.AddAuditLog(auditLog);
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
            var countryOrder = await _context.CountryOrders
                .Include(co => co.Orders) // Include related orders
                .FirstOrDefaultAsync(co => co.Id == id);

            if (countryOrder == null)
            {
                return NotFound();
            }

            if (countryOrder.Orders.Any()) // Check if any orders are referencing this CountryOrder
            {
                ModelState.AddModelError("", "Cannot delete this CountryOrder as it is referenced by one or more orders.");
                return View(countryOrder); // Return to the delete view with error message
            }

            _context.CountryOrders.Remove(countryOrder);
            await _context.SaveChangesAsync();

            // Audit Log
            var auditLog = new AuditLog
            {
                Action = "Deleted",
                Entity = "CountryOrder",
                EntityId = countryOrder.Id,
                PerformedBy = User.Identity.Name,
                PerformedAt = DateTime.UtcNow
            };
            await _auditLogRepo.AddAuditLog(auditLog);

            return RedirectToAction(nameof(Index));
        }

        private bool CountryOrderExists(int id)
        {
            return (_context.CountryOrders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }

}
