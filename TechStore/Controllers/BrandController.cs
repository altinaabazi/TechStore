﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TechStore.Models;
using TechStore.Repositories;

namespace TechStore.Controllers
{
    public class BrandController : Controller
    {
        private readonly IBrandRepository _brandRepo;
        private readonly IAuditLogRepository _auditLogRepo;

        public BrandController(IBrandRepository brandRepo, IAuditLogRepository auditLogRepo)
        {
            _brandRepo = brandRepo;
            _auditLogRepo = auditLogRepo;
        }


        // GET: Brand
        public async Task<IActionResult> Index()
        {
            var brands = await _brandRepo.GetBrands();
            return View(brands);
        }

        // GET: Brand/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _brandRepo.GetBrandById(id.Value);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                await _brandRepo.AddBrand(brand);

                // Audit Log
                var auditLog = new AuditLog
                {
                    Action = "Added",
                    Entity = "Brand",
                    EntityId = brand.Id,
                    PerformedBy = User.Identity.Name,
                    PerformedAt = DateTime.UtcNow
                };
                await _auditLogRepo.AddAuditLog(auditLog);

                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }


        // GET: Brand/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _brandRepo.GetBrandById(id.Value);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // POST: Brand/Edit/5
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
                    await _brandRepo.UpdateBrand(brand);

                    // Audit Log
                    var auditLog = new AuditLog
                    {
                        Action = "Updated",
                        Entity = "Brand",
                        EntityId = brand.Id,
                        PerformedBy = User.Identity.Name,
                        PerformedAt = DateTime.UtcNow
                    };
                    await _auditLogRepo.AddAuditLog(auditLog);
                }
                catch
                {
                    return View(brand);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }


        // GET: Brand/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _brandRepo.GetBrandById(id.Value);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brand = await _brandRepo.GetBrandById(id);
            if (brand == null)
            {
                return NotFound();
            }

            await _brandRepo.DeleteBrand(brand);

            // Audit Log
            var auditLog = new AuditLog
            {
                Action = "Deleted",
                Entity = "Brand",
                EntityId = brand.Id,
                PerformedBy = User.Identity.Name,
                PerformedAt = DateTime.UtcNow
            };
            await _auditLogRepo.AddAuditLog(auditLog);

            return RedirectToAction(nameof(Index));
        }



    }
}
