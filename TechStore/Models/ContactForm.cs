using System.ComponentModel.DataAnnotations;

namespace TechStore.Models
{
    public class ContactFormModel
    {
        // Kjo është fusha që do të shërbejë si Primary Key
        public int Id { get; set; }

        [Required(ErrorMessage = "Emri është i detyrueshëm.")]
        [StringLength(100, ErrorMessage = "Emri mund të ketë maksimum 100 karaktere.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email-i është i detyrueshëm.")]
        [EmailAddress(ErrorMessage = "Ju lutem futni një adresë të vlefshme email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mesazhi është i detyrueshëm.")]
        [StringLength(1000, ErrorMessage = "Mesazhi mund të ketë maksimum 1000 karaktere.")]
        public string Message { get; set; }
    }
}

