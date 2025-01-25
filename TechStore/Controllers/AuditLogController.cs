using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechStore.Repositories;

namespace TechStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuditLogController : Controller
    {
        private readonly IAuditLogRepository _auditLogRepo;

        public AuditLogController(IAuditLogRepository auditLogRepo)
        {
            _auditLogRepo = auditLogRepo;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;  // Set the number of logs per page
            var logs = await _auditLogRepo.GetAuditLogs();  // Get all audit logs

            var totalLogs = logs.Count();
            var totalPages = (int)Math.Ceiling((double)totalLogs / pageSize);

            // Skip the logs from previous pages and take the logs for the current page
            var logsToShow = logs.Skip((page - 1) * pageSize).Take(pageSize);

            // Pass the logs and pagination information to the view
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;

            return View(logsToShow);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteAll()
        {
            await _auditLogRepo.DeleteAllAuditLogs();  // Deletes all logs

            // You can redirect the user after deletion or show a success message
            return RedirectToAction("Index");  // Redirect back to the Index view
        }
    }
}
