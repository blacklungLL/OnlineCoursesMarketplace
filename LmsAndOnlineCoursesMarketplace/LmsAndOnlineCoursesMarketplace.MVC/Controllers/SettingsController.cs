using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LmsAndOnlineCoursesMarketplace.Persistence.Contexts;
using LmsAndOnlineCoursesMarketplace.MVC.Models.Profile;

namespace LmsAndOnlineCoursesMarketplace.MVC.Controllers
{
    public class SettingsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public SettingsController(
            UserManager<IdentityUser> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var identityUser = await _userManager.GetUserAsync(User);
            if (identityUser == null)
                return Challenge();

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);

            if (user == null)
                return NotFound();

            var model = new EditProfileVM
            {
                Name = user.Name,
                JobPosition = user.JobPosition,
                Description = user.Description
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(EditProfileVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var identityUser = await _userManager.GetUserAsync(User);
            if (identityUser == null)
                return Challenge();

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);

            if (user == null)
                return NotFound();

            user.Name = model.Name;
            user.JobPosition = model.JobPosition;
            user.Description = model.Description;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Profile");
        }
    }
}