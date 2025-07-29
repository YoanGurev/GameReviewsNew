using System.ComponentModel.DataAnnotations;

namespace GameReviews.Models.ViewModels
{
    public class ReviewViewModel
    {
        public int GameId { get; set; }

        [Required]
        [Range(1, 10)]
        public int Rating { get; set; }

        [Required]
        [StringLength(1000)]
        public string Content { get; set; }
    }
}

