using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechStore.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string Action { get; set; } // Shto, Ndrysho, Fshi
        public string Entity { get; set; } // produkti, Kategoria, etj.
        public int EntityId { get; set; } // ID e entitetit të ndryshuar
        public string PerformedBy { get; set; } // Emaili ose Emri i adminit që kreu veprimin
        public DateTime PerformedAt { get; set; } // Koha e kryerjes së veprimit
    }

}
