using Microsoft.AspNetCore.Mvc;
using TechStore.Models;
using TechStore.Models.DTOs;
using TechStore.Repositories;

public class StockController : Controller
{
    private readonly IStockRepository _stockRepo;
    private readonly IAuditLogRepository _auditLogRepo;

    // Make sure only this constructor exists, and that both repositories are injected
    public StockController(IStockRepository stockRepo, IAuditLogRepository auditLogRepo)
    {
        _stockRepo = stockRepo;
        _auditLogRepo = auditLogRepo;
    }

    public async Task<IActionResult> Index(string sTerm = "")
    {
        var stocks = await _stockRepo.GetStocks(sTerm);
        return View(stocks);
    }

    public async Task<IActionResult> ManangeStock(int productId)
    {
        var existingStock = await _stockRepo.GetStockByProductId(productId);
        var stock = new StockDTO
        {
            ProductId = productId,
            Quantity = existingStock != null ? existingStock.Quantity : 0
        };
        return View(stock);
    }

    [HttpPost]
    public async Task<IActionResult> ManangeStock(StockDTO stock)
    {
        if (!ModelState.IsValid)
            return View(stock);

        if (stock.Quantity < 0)
        {
            TempData["errorMessage"] = "Quantity cannot be negative.";
            return View(stock);
        }

        try
        {
            // Save stock changes
            await _stockRepo.ManageStock(stock);

            // Log action in AuditLog
            var auditLog = new AuditLog
            {
                Action = "Update Stock",  // or any relevant action like "Manage Stock"
                Entity = "Stock",         // Entity is Stock
                EntityId = stock.ProductId,  // ID of the product being modified
                PerformedBy = User.Identity.Name,  // Admin's identity
                PerformedAt = DateTime.UtcNow   // Timestamp of the action
            };
            await _auditLogRepo.AddAuditLog(auditLog);  // Save to the Audit Log

            TempData["successMessage"] = "Stock is updated successfully.";
        }
        catch (Exception)
        {
            TempData["errorMessage"] = "Something went wrong!";
        }

        return RedirectToAction(nameof(Index));
    }
}
