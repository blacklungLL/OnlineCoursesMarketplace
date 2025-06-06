using LmsAndOnlineCoursesMarketplace.MVC.Models.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LmsAndOnlineCoursesMarketplace.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using LmsAndOnlineCoursesMarketplace.MVC.Models.Profile;
using LmsAndOnlineCoursesMarketplace.MVC.Models.ShoppingCart;

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
                Description = user.Description,
                ReviewsCnt = user.ReviewsCnt,
                SubscriptionsCnt = user.SubscriptionsCnt,
                CreatedCourses = user.Courses?
                    .Select(c => new CourseVM
                    {
                        Id = c.Id,
                        Title = c.Title,
                        ImageLink = c.ImageLink,
                        Price = c.Price,
                        Category = c.Category,
                        Language = c.Language,
                        UserId = c.UserId
                    })
                    .ToList() ?? new List<CourseVM>()
            };
            
            var purchased = await _context.UserCoursePurchases
                .Include(up => up.Course)
                .ThenInclude(c => c.User) // связь с автором курса
                .Where(up => up.UserId == user.Id)
                .Select(up => up.Course)
                .ToListAsync();

            model.PurchasedCourses = purchased.Select(c => new CourseSummaryVM
            {
                Id = c.Id,
                Title = c.Title,
                ImageLink = c.ImageLink,
                Category = c.Category,
                Language = c.Language,
                Duration = c.Duration,
                Views = c.Views,
                Price = c.Price,
                AuthorName = c.User?.Name ?? "Unknown"
            }).ToList();
            
            if (identityUser != null)
            {
                var userForViewBag = await _context.Users
                    .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);

                if (userForViewBag != null)
                {
                    ViewBag.UserName = user.Name;
                    ViewBag.JobPosition = user.JobPosition;
                    ViewBag.EnrollStudents = user.EnrollStudents;
                    ViewBag.CoursesCnt = user.CoursesCnt;
                    ViewBag.UserEmail = user.Email;
                }
            }
            
            return View(model);
        }
    }
}