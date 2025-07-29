using System.ComponentModel.DataAnnotations;

namespace GameReviews.Models
{
    public class Game
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }

        [Range(0, 10)]
        public double Rating { get; set; }

        [Range(0, 1000)]
        public decimal Price { get; set; }   

        public int GenreId { get; set; }
        public Genre Genre { get; set; }

        public int PlatformId { get; set; }
        public Platform Platform { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    }
}



