using System;
using System.ComponentModel.DataAnnotations;

namespace GameReviews.Models
{
    public class ContactForm
    {
        public int Id { get; set; }

        
        public string Name { get; set; } = null!;

        
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(100)]
        public string Subject { get; set; } = null!;

        [Required]
        [StringLength(2000)]
        public string Message { get; set; } = null!;

        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;

        public string? ReplyMessage {  get; set; }
        public DateTime? RepliedAt { get; set; }
        public string? RepliedByUserId { get; set; }
        public string? SubmittedByUserId { get; set; }
        
    }
}

