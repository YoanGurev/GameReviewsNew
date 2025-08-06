# 🎮 GameReviews

**GameReviews** is a modern ASP.NET Core MVC web application for discovering, reviewing, and favoriting video games. Built with .NET 8, Entity Framework Core, and Razor Pages, it features user authentication, an admin area, and dynamic UI elements like search, filtering, pagination, and AJAX-based interactivity.


## 🚀 Features

- 🧑‍💻 **User Authentication & Authorization** (Register/Login/Logout)
- 🎮 **Game Listing & Detail Pages**
  - Search by title
  - Filter by genre and platform
  - Pagination support
- ⭐ **Favorites System**
  - Add/remove games from personal favorites
  - Favorites page with pagination and success messages
- 📝 **Review System**
  - Post reviews with ratings (1–10)
  - View all user reviews per game
  - Upvote/downvote individual reviews
- 📬 **Contact Page**
  - Users can send messages to admins
- 📥 **Admin Inbox (Inbox view)**
  - Admins can view contact messages with pagination
  - Mark messages as read/unread
- 🛠️ **Admin Area**
  - Add/Edit/Delete games
  - Manage genres and platforms (seeded by default)


### 💻 Technologies Used

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- Razor Pages
- Identity (Custom Register/Login pages)
- SQL Server (localdb)
- Bootstrap 5 (custom theme with Bootstrap Icons)
- LINQ, Tag Helpers, Partial Views


#### 🧪 How to Run the Project Locally

1. **Clone the Repository**
git clone https://github.com/YoanGurev/GameReviewsNew/tree/master
cd GameReviews

2 **Open in Visual Studio 2022/JetBrains Project Rider**

3 **Apply Migrations**

- Open Package Manager Console
- Run 'Update-Database'

4 **Run the Application**


##### 🧑‍⚖️ Admin Login (for testing)
- Email: admin@gamereviews.com
- Password: Admin123!

