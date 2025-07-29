using Microsoft.AspNetCore.Identity;

namespace GameReviews.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}

