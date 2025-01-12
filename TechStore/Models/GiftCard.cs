using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechStore.Models
{
    [Table("GiftCard")]
    public class GiftCard
    {
        public int Id { get; set; }


        [Required]
        public int GiftCardId { get; set; }
        public GiftCardPrice GiftCardPrice { get; set; }

        public DateTime ExpirationDate { get; set; }  // Data e skadimit

        // Lidhja me ShoppingCart
        public int? ShoppingCartId { get; set; }  // Foreign Key që lidh me ShoppingCart
        [ForeignKey("ShoppingCartId")]
        public ShoppingCart ShoppingCart { get; set; }
    }
}
