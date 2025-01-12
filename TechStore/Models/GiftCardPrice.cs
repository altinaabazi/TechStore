using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TechStore.Models
{

    [Table("GiftCardPrice")]
    public class GiftCardPrice
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public int Price { get; set; }

    }
}
