using System.ComponentModel.DataAnnotations;
namespace TechStore.Models.DTOs
{
    public class BrandDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string BrandName { get; set; }
    }
}