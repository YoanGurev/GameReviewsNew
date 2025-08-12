using System.ComponentModel.DataAnnotations;

namespace GameReviews.Models
{
    public class GameRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        [Range(0, 1000, ErrorMessage = "Price must be between 0 and 1000")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Genre is required")]
        public int GenreId { get; set; }
        
        public Genre? Genre { get; set; }
        [Required(ErrorMessage = "Platform is required")]
        public int PlatformId { get; set; }
        public Platform? Platform { get; set; }

        [Url(ErrorMessage = "Please enter a valid image URL.")]
        public string? ImageUrl { get; set; }

        [Url(ErrorMessage = "Please enter a valid video URL.")]
        public string? VideoUrl { get; set; }


        public string? RequestedByUserId { get; set; }
        public ApplicationUser? RequestedByUser { get; set; }

        public string Status { get; set; } = "Pending";

        public string? AdminResponse { get; set; }
        public DateTime? ResponseDate { get; set; }
    }
}

