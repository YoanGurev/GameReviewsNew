using System.ComponentModel.DataAnnotations;

namespace GameReviews.Models
{
    public class Genre
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        public ICollection<Game> Games { get; set; } = new List<Game>();
    }
}

