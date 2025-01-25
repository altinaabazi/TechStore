using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TechStore.Data;
using TechStore.Models;

namespace TechStore.Repositories
{
    public interface IAuditLogRepository
    {
        Task AddAuditLog(AuditLog auditLog);
        Task<List<AuditLog>> GetAuditLogs(); // Merr të gjitha logjet

        Task DeleteAllAuditLogs();  // Add this method to delete all logs
    }
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly ApplicationDbContext _context;

        public AuditLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAuditLog(AuditLog auditLog)
        {
            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AuditLog>> GetAuditLogs()
        {
            return await _context.AuditLogs.OrderByDescending(log => log.PerformedAt).ToListAsync();
        }

        public async Task DeleteAllAuditLogs()
        {
            var allLogs = _context.AuditLogs;
            _context.AuditLogs.RemoveRange(allLogs); // Removes all logs
            await _context.SaveChangesAsync(); // Save changes to the database
        }
    }
}
