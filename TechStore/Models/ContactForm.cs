using System.ComponentModel.DataAnnotations;

namespace TechStore.Models
{
    public class ContactFormModel
    {
        // Kjo �sht� fusha q� do t� sh�rbej� si Primary Key
        public int Id { get; set; }

        [Required(ErrorMessage = "Emri �sht� i detyruesh�m.")]
        [StringLength(100, ErrorMessage = "Emri mund t� ket� maksimum 100 karaktere.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email-i �sht� i detyruesh�m.")]
        [EmailAddress(ErrorMessage = "Ju lutem futni nj� adres� t� vlefshme email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mesazhi �sht� i detyruesh�m.")]
        [StringLength(1000, ErrorMessage = "Mesazhi mund t� ket� maksimum 1000 karaktere.")]
        public string Message { get; set; }
    }
}

