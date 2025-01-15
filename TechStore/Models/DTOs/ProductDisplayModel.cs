using Humanizer.Localisation;
using TechStore.Data;
namespace TechStore.Models.DTOs
{
    public class ProductDisplayModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
        public string STerm { get; set; } = "";
        public int BrandId { get; set; } = 0;
    }
}
