namespace GameReviews.Models.ViewModels
{
    public class FavoritesViewModel
    {
        public int GameId { get; set; }
        public string Title { get; set; }
        public string GenreName { get; set; }
        public string PlatformName { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
    }
}

