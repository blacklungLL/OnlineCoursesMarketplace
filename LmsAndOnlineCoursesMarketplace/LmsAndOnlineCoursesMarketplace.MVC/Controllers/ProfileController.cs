using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LmsAndOnlineCoursesMarketplace.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using LmsAndOnlineCoursesMarketplace.MVC.Models.Profile;

namespace LmsAndOnlineCoursesMarketplace.MVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public ProfileController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var identityUser = await _userManager.GetUserAsync(User);

            if (identityUser == null)
                return Challenge();
            
            var user = await _context.Users
                .Include(u => u.Courses)
                .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);

            if (user == null)
                return NotFound();
            
            var model = new ProfileVM
            {
                Name = user.Name,
                Email = user.Email,
                JobPosition = user.JobPosition,
                CoursesCnt = user.CoursesCnt,
                EnrollStudents = user.EnrollStudents,
                ReviewsCnt = user.ReviewsCnt,
                SubscriptionsCnt = user.SubscriptionsCnt
            };
            
            return View(model);
        }
    }
}