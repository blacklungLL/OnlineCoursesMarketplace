using LmsAndOnlineCoursesMarketplace.Domain.Entities;
using LmsAndOnlineCoursesMarketplace.MVC.Models.OtherUserProfile;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LmsAndOnlineCoursesMarketplace.Persistence.Contexts;
using LmsAndOnlineCoursesMarketplace.MVC.Models.Profile;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
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
            
            User? curUser = null;

            if (identityUser != null)
            {
                curUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);
            }
            
            var currentProfile = await _context.Users
                .Include(u => u.Subscriptions)
                .ThenInclude(us => us.SubscribedTo)
                .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);
            
            if (curUser != null)
            {
                ViewBag.CurrentUserId = curUser.Id;
                ViewBag.CurrentUserName = curUser.Name;
                ViewBag.CurrentJobPosition = curUser.JobPosition;
                ViewBag.CurrentSubscriptionsCnt = curUser.SubscriptionsCnt;
                ViewBag.CurrentEmail = curUser.Email;
                ViewBag.Subscriptions = currentProfile.Subscriptions?
                    .Select(us => new SubscriptionPreviewVM()
                    {
                        Id = us.SubscribedToId,
                        Name = us.SubscribedTo?.Name ?? "Unknown",
                    }).ToList() ?? new List<SubscriptionPreviewVM>();
            }

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