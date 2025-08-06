using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameReviews.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; } = null!;

        [Range(1, 5)]
        public int Rating { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        
        [Required]
        public string UserId { get; set; } = null!;
        [Required]
        public int GameId { get; set; }

        
        public ApplicationUser? User { get; set; }
        public Game? Game { get; set; }
        public int Upvotes { get; set; } = 0;
        public int Downvotes { get; set; } = 0;
        public ICollection<ReviewVote> Votes { get; set; } = new List<ReviewVote>();


    }
}

