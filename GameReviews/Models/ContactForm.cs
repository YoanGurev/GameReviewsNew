using System;
using System.ComponentModel.DataAnnotations;

namespace GameReviews.Models
{
    public class ContactForm
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Subject { get; set; } = null!;

        [Required]
        [StringLength(2000)]
        public string Message { get; set; } = null!;

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;
    }
}

