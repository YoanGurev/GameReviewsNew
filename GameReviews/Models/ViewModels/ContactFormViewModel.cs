using System.ComponentModel.DataAnnotations;

namespace GameReviews.Models.ViewModels
{
    public class ContactFormViewModel
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Enter a valid email.")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "Subject is required")]
        [StringLength(100, ErrorMessage = "Subject is too long.")]
        public string Subject { get; set; } = null!;

        [Required(ErrorMessage = "Message is required.")]
        [StringLength(2000, ErrorMessage = "Message is too long.")]
        public string Message { get; set; } = null!;
    }
}

