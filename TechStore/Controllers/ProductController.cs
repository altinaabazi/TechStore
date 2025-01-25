using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechStore.Constants;
using TechStore.Repositories;
using TechStore.Models;
using TechStore.Models.DTOs;
using TechStore.Services;
using System;

namespace TechStore.Controllers;

public class ProductController : Controller
{
    private readonly IProductRepository _productRepo;
    private readonly IBrandRepository _brandRepo;
    private readonly ICategoryRepository _categoryRepo;
    private readonly IFileService _fileService;
    private readonly IAuditLogRepository _auditLogRepo;

    public ProductController(IProductRepository productRepo, IBrandRepository brandRepo, ICategoryRepository categoryRepo, IFileService fileService, IAuditLogRepository auditLogRepo)
    {
        _productRepo = productRepo;
        _brandRepo = brandRepo;
        _categoryRepo = categoryRepo;
        _fileService = fileService;
        _auditLogRepo = auditLogRepo;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        int pageSize = 5;
        var products = await _productRepo.GetProducts();

        var totalProducts = products.Count();
        var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize);

        var productsToShow = products.Skip((page - 1) * pageSize).Take(pageSize);

        foreach (var product in productsToShow)
        {
            product.Category ??= new Category { Name = "Uncategorized" };
        }

        var model = new ProductDisplayModel
        {
            Products = productsToShow,
            Brands = await _brandRepo.GetBrands(),
            Categories = await _categoryRepo.GetCategories(),
            STerm = "",
            BrandId = 0
        };

        ViewBag.TotalPages = totalPages;
        ViewBag.CurrentPage = page;

        return View(model);
    }

    public async Task<IActionResult> AddProduct()
    {
        var brandSelectList = (await _brandRepo.GetBrands()).Select(brand => new SelectListItem
        {
            Text = brand.Name,
            Value = brand.Id.ToString(),
        });

        var categorySelectList = (await _categoryRepo.GetCategories()).Select(category => new SelectListItem
        {
            Text = category.Name,
            Value = category.Id.ToString(),
        });

        ProductDTO productToAdd = new()
        {
            BrandList = brandSelectList,
            CategoryList = categorySelectList
        };

        return View(productToAdd);
    }

    [HttpPost]
    public async Task<IActionResult> AddProduct(ProductDTO productToAdd)
    {
        var brandSelectList = (await _brandRepo.GetBrands()).Select(brand => new SelectListItem
        {
            Text = brand.Name,
            Value = brand.Id.ToString(),
        });
        productToAdd.BrandList = brandSelectList;

        var categorySelectList = (await _categoryRepo.GetCategories()).Select(category => new SelectListItem
        {
            Text = category.Name,
            Value = category.Id.ToString(),
        });
        productToAdd.CategoryList = categorySelectList;

        if (!ModelState.IsValid)
            return View(productToAdd);

        try
        {
            if (productToAdd.ImageFile != null)
            {
                if (productToAdd.ImageFile.Length > 1 * 1024 * 1024)
                {
                    throw new InvalidOperationException("Image file cannot exceed 1 MB");
                }

                string[] allowedExtensions = new[] { ".jpeg", ".jpg", ".png" };
                string imageName = await _fileService.SaveFile(productToAdd.ImageFile, allowedExtensions);
                productToAdd.Image = imageName;
            }

            Product product = new()
            {
                ProductName = productToAdd.ProductName,
                Image = productToAdd.Image,
                BrandId = productToAdd.BrandId,
                CategoryId = productToAdd.CategoryId,
                Price = productToAdd.Price
            };

            await _productRepo.AddProduct(product);

            // Audit Log
            var auditLog = new AuditLog
            {
                Action = "Added",
                Entity = "Product",
                EntityId = product.Id,
                PerformedBy = User.Identity.Name,
                PerformedAt = DateTime.UtcNow
            };
            await _auditLogRepo.AddAuditLog(auditLog);

            TempData["successMessage"] = "Product is added successfully";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["errorMessage"] = ex.Message;
            return View(productToAdd);
        }
    }

    public async Task<IActionResult> UpdateProduct(int id)
    {
        var product = await _productRepo.GetProductById(id);
        if (product == null)
        {
            TempData["errorMessage"] = $"Product with the id: {id} not found";
            return RedirectToAction(nameof(Index));
        }

        var brandSelectList = (await _brandRepo.GetBrands()).Select(brand => new SelectListItem
        {
            Text = brand.Name,
            Value = brand.Id.ToString(),
            Selected = brand.Id == product.BrandId
        });

        var categorySelectList = (await _categoryRepo.GetCategories()).Select(category => new SelectListItem
        {
            Text = category.Name,
            Value = category.Id.ToString(),
            Selected = category.Id == product.CategoryId
        });

        ProductDTO productToUpdate = new()
        {
            Id = product.Id,
            ProductName = product.ProductName,
            BrandId = product.BrandId,
            CategoryId = product.CategoryId,
            Price = product.Price,
            Image = product.Image,
            BrandList = brandSelectList,
            CategoryList = categorySelectList
        };

        return View(productToUpdate);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateProduct(ProductDTO productToUpdate)
    {
        var brandSelectList = (await _brandRepo.GetBrands()).Select(brand => new SelectListItem
        {
            Text = brand.Name,
            Value = brand.Id.ToString(),
            Selected = brand.Id == productToUpdate.BrandId
        });
        productToUpdate.BrandList = brandSelectList;

        var categorySelectList = (await _categoryRepo.GetCategories()).Select(category => new SelectListItem
        {
            Text = category.Name,
            Value = category.Id.ToString(),
        });
        productToUpdate.CategoryList = categorySelectList;

        if (!ModelState.IsValid)
            return View(productToUpdate);

        try
        {
            string oldImage = productToUpdate.Image;

            if (productToUpdate.ImageFile != null)
            {
                if (productToUpdate.ImageFile.Length > 1 * 1024 * 1024)
                {
                    throw new InvalidOperationException("Image file cannot exceed 1 MB");
                }

                string[] allowedExtensions = new[] { ".jpeg", ".jpg", ".png" };
                string imageName = await _fileService.SaveFile(productToUpdate.ImageFile, allowedExtensions);
                oldImage = productToUpdate.Image;
                productToUpdate.Image = imageName;
            }

            Product product = new()
            {
                Id = productToUpdate.Id,
                ProductName = productToUpdate.ProductName,
                BrandId = productToUpdate.BrandId,
                CategoryId = productToUpdate.CategoryId,
                Price = productToUpdate.Price,
                Image = productToUpdate.Image
            };

            await _productRepo.UpdateProduct(product);

            // Audit Log
            var auditLog = new AuditLog
            {
                Action = "Updated",
                Entity = "Product",
                EntityId = product.Id,
                PerformedBy = User.Identity.Name,
                PerformedAt = DateTime.UtcNow
            };
            await _auditLogRepo.AddAuditLog(auditLog);

            if (!string.IsNullOrWhiteSpace(oldImage) && oldImage != productToUpdate.Image)
            {
                _fileService.DeleteImage(oldImage);
            }

            TempData["successMessage"] = "Product is updated successfully";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["errorMessage"] = ex.Message;
            return View(productToUpdate);
        }
    }

    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            var product = await _productRepo.GetProductById(id);
            if (product == null)
            {
                TempData["errorMessage"] = $"Product with the id: {id} not found";
                return RedirectToAction(nameof(Index));
            }

            await _productRepo.DeleteProduct(product);

            // Audit Log
            var auditLog = new AuditLog
            {
                Action = "Deleted",
                Entity = "Product",
                EntityId = product.Id,
                PerformedBy = User.Identity.Name,
                PerformedAt = DateTime.UtcNow
            };
            await _auditLogRepo.AddAuditLog(auditLog);

            if (!string.IsNullOrWhiteSpace(product.Image))
            {
                _fileService.DeleteImage(product.Image);
            }

            TempData["successMessage"] = "Product deleted successfully";
        }
        catch (Exception ex)
        {
            TempData["errorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _productRepo.GetProductById(id);
        if (product == null)
        {
            TempData["errorMessage"] = $"Product with the id: {id} not found";
            return RedirectToAction(nameof(Index));
        }

        return View(product);
    }
}
