using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TechStore.Data;

namespace TechStore.Models
{
    [Table("ShoppingCart")]
    public class ShoppingCart
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        public bool IsDeleted { get; set; } = false;

        public ICollection<CartDetail> CartDetails { get; set; }
        public ApplicationUser User { get; set; } // Lidhja me tabelën AspNetUsers


    }
}
