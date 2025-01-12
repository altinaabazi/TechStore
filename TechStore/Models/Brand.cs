using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechStore.Models
{
    [Table("Brand")]
    public class Brand
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public string? Description { get; set; }

        // Relationship: One Brand -> Many Products
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
