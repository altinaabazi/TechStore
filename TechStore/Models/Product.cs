using Humanizer.Localisation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace TechStore.Models
{
    [Table("Product")]
    public class Product
    {

        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string? ProductName { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }


        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public double Price { get; set; }
        public string? Image { get; set; }
        [Required]
        public int BrandId { get; set; }
        public Brand Brand { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }
        public List<CartDetail> CartDetail { get; set; }
        public Stock Stock { get; set; }

        [NotMapped]
        public string BrandName { get; set; }
        [NotMapped]
        public int Quantity { get; set; }
    }
}
