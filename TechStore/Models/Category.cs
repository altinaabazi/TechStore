using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechStore.Models
{
    [Table("Category")]
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public string? Description { get; set; }

        // Relationship: One Category -> Many Products
        public List<Product> Products { get; set; } = new List<Product>();
    }
}
