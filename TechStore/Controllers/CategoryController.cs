using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechStore.Models;
using TechStore.Repositories;

namespace TechStore.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _catoRepo;
        private readonly IAuditLogRepository _auditLogRepo;

        public CategoryController(ICategoryRepository catoRepo, IAuditLogRepository auditLogRepo)
        {
            _catoRepo = catoRepo;
            _auditLogRepo = auditLogRepo;
        }
        [Authorize(Roles = "Manager,Admin")]
        // GET: Category
        public async Task<IActionResult> Index()
        {
            var categories = await _catoRepo.GetCategories();
            return View(categories);
        }

        [Authorize(Roles = "Manager,Admin")]
        // GET: Category/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _catoRepo.GetCategoryById(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        [Authorize(Roles = "Admin")]

        // GET: Category/Create
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Category category)
        {
            if (ModelState.IsValid)
            {
                await _catoRepo.AddCategory(category);

                // Audit Log
                var auditLog = new AuditLog
                {
                    Action = "Added",
                    Entity = "Category",
                    EntityId = category.Id,
                    PerformedBy = User.Identity.Name,
                    PerformedAt = DateTime.UtcNow
                };
                await _auditLogRepo.AddAuditLog(auditLog);

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }
        [Authorize(Roles = "Admin")]

        // GET: Category/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _catoRepo.GetCategoryById(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        [Authorize(Roles = "Admin")]

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _catoRepo.UpdateCategory(category);

                    // Audit Log
                    var auditLog = new AuditLog
                    {
                        Action = "Updated",
                        Entity = "Category",
                        EntityId = category.Id,
                        PerformedBy = User.Identity.Name,
                        PerformedAt = DateTime.UtcNow
                    };
                    await _auditLogRepo.AddAuditLog(auditLog);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating the category.");
                    return View(category);
                }
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }
        [Authorize(Roles = "Admin")]

        // GET: Category/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _catoRepo.GetCategoryById(id.Value);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        [Authorize(Roles = "Admin")]

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _catoRepo.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }

            await _catoRepo.DeleteCategory(category);

            // Audit Log
            var auditLog = new AuditLog
            {
                Action = "Deleted",
                Entity = "Category",
                EntityId = category.Id,
                PerformedBy = User.Identity.Name,
                PerformedAt = DateTime.UtcNow
            };
            await _auditLogRepo.AddAuditLog(auditLog);

            return RedirectToAction(nameof(Index));
        }
    }
}
