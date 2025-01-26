using Microsoft.AspNetCore.Mvc;
using TechStore.Models;
using TechStore.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechStore.Data;
using Microsoft.AspNetCore.Authorization;

namespace TechStore.Controllers
{
    [Authorize]
    public class BrandController : Controller
    {
        private readonly IBrandRepository _brandRepo;
        private readonly IAuditLogRepository _auditLogRepo;
        private readonly RedisCacheService _redisCache;
        private readonly ApplicationDbContext _context;




        public BrandController(IBrandRepository brandRepo, IAuditLogRepository auditLogRepo, RedisCacheService redisCache, ApplicationDbContext context)
        {
            _brandRepo = brandRepo;
            _auditLogRepo = auditLogRepo;
            _redisCache = redisCache; // Inject RedisCacheService
            _context = context;
        }

        // GET: Brand
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Index()
        {
            var brandsCacheKey = "brands";
            var brands = await _redisCache.GetValueAsync(brandsCacheKey);

            if (string.IsNullOrEmpty(brands))
            {
                var brandList = await _brandRepo.GetBrands();
                if (brandList.Count() > 0)
                {
                    var serializedBrands = JsonConvert.SerializeObject(brandList);
                    await _redisCache.SetValueAsync(brandsCacheKey, serializedBrands, TimeSpan.FromMinutes(5)); // Cache for 5 minutes
                    Console.WriteLine("Brands saved to Redis successfully.");
                }

                return View(brandList);
            }
            else
            {
                Console.WriteLine("Brands retrieved from Redis.");
                try
                {
                    var brandList = JsonConvert.DeserializeObject<List<Brand>>(brands);
                    if (brandList == null)
                    {
                        return View(new List<Brand>());
                    }
                    return View(brandList);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error deserializing Redis data: " + ex.Message);
                    return View(new List<Brand>());
                }
            }
        }
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Brands == null)
            {
                return NotFound();
            }

            var orderStatus = await _context.Brands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderStatus == null)
            {
                return NotFound();
            }

            return View(orderStatus);
        }

        [Authorize(Roles = "Admin")]
        // GET: Brand/Create
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
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

                // Clear the brand cache to reflect the new brand added
                await _redisCache.SetValueAsync("brands", null);

                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        [Authorize(Roles = "Admin")]
        // GET: Brand/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var brand = await _brandRepo.GetBrandById(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        [Authorize(Roles = "Admin")]
        // POST: Brand/Edit
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

                    // Clear the brand cache after update
                    await _redisCache.SetValueAsync("brands", null);
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _brandRepo.GetBrandById(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }
        [Authorize(Roles = "Admin")]
        // POST: Brand/Delete
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

            // Clear the brand cache after deletion
            await _redisCache.SetValueAsync("brands", null);

            return RedirectToAction(nameof(Index));
        }
    }
}