using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace TechStore.Models.DTOs;
public class ProductDTO
{
    public int Id { get; set; }
    [Required]
    [MaxLength(40)]
    public string? ProductName { get; set; }
  
    [Required]
    public double Price { get; set; }
    public string? Image { get; set; }
    [Required]
    public IFormFile? ImageFile { get; set; }
    public int BrandId { get; set; }
    public IEnumerable<SelectListItem>? BrandList { get; set; }

    public int CategoryId { get; set; }
    public IEnumerable<SelectListItem>? CategoryList { get; set; }

}