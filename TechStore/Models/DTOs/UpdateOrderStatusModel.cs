using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;

namespace TechStore.Models.DTOs
{
    public class UpdateOrderStatusModel
    {
        public int OrderId { get; set; }

        [Required]
        public int OrderStatusId { get; set; }

        public IEnumerable<SelectListItem>? OrderStatusList { get; set; }
    }
}
