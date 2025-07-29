using System.ComponentModel.DataAnnotations;

namespace GameReviews.Models
{
    public class Favorite
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}

