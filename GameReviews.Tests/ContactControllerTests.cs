using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameReviews.Controllers;
using GameReviews.Data;
using GameReviews.Models;
using GameReviews.Models.ViewModels;
using System.Threading.Tasks;
using System.Linq;

namespace GameReviews.Tests.Controllers
{

    public class ContactControllerTests
    {

        private ApplicationDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "ContactFormTestDb_" + System.Guid.NewGuid())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task Submit_ValidModel_SavesToDatabaseAndRedirects()
        {
            
            var dbContext = GetInMemoryDbContext();
            var controller = new ContactController(dbContext);

            var model = new ContactFormViewModel
            {
                Name = "Test",
                Email = "test@example.com",
                Subject = "Test Subject",
                Message = "This is a test message"
            };

            
            var result = await controller.Submit(model);

            
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Contact", redirect.ActionName);

            var contactInDb = dbContext.ContactForms.FirstOrDefault();
            Assert.NotNull(contactInDb);
            Assert.Equal("Test User", contactInDb.Name);
        }

        [Fact]
        public async Task Submit_InvalidModel_ReturnsViewWithModel()
        {
            
            var dbContext = GetInMemoryDbContext();
            var controller = new ContactController(dbContext);
            controller.ModelState.AddModelError("Email", "Required");

            var model = new ContactFormViewModel
            {
                Name = "Invalid",
                Subject = "Missing Email",
                Message = "Oops"
            };

            
            var result = await controller.Submit(model);

            
            var view = Assert.IsType<ViewResult>(result);
            var returnedModel = Assert.IsType<ContactFormViewModel>(view.Model);
            Assert.Equal("Invalid", returnedModel.Name);
        }
    }
}
