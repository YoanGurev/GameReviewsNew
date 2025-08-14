using GameReviews.Models;
using System.Collections.Generic;

namespace GameReviews.Models.ViewModels
{
    public class UserInboxViewModel
    {
        public IEnumerable<ContactForm> ContactMessages { get; set; } = new List<ContactForm>();
        public IEnumerable<GameRequest> GameRequests { get; set; } = new List<GameRequest>();
    }
}

