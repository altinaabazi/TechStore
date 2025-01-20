using System.ComponentModel.DataAnnotations;

namespace TechStore.Models
{
    public class CountryOrder
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }

        public List<Order> Orders { get; set; } = new List<Order>();
    }
}
