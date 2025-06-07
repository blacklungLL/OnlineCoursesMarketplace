using LmsAndOnlineCoursesMarketplace.Domain.Entities;
using LmsAndOnlineCoursesMarketplace.MVC.Models.Course;
using LmsAndOnlineCoursesMarketplace.MVC.Models.OtherUserProfile;
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
                .Include(u => u.Subscribers)
                .Include(u => u.Subscriptions)
                .ThenInclude(us => us.SubscribedTo)
                .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);

            if (user == null)
                return NotFound();

            var model = new ProfileVM
            {
                UserId = user.Id,
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
                    .ToList() ?? new List<CourseVM>(),
                
                Subscriptions = user.Subscriptions?
                    .Select(us => us.SubscribedTo)
                    .Select(u => new SubscriptionPreviewVM
                    {
                        Id = u.Id,
                        Name = u.Name,
                        JobPosition = u.JobPosition,
                        EnrollStudents = u.EnrollStudents,
                        CourseCnt = u.CoursesCnt
                    }).ToList() ?? new List<SubscriptionPreviewVM>()
            };
            
            var purchased = await _context.UserCoursePurchases
                .Include(up => up.Course)
                .ThenInclude(c => c.User)
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
            
            var subscriptions = await _context.UserSubscriptions
                .Where(us => us.SubscriberId == user.Id)
                .Select(us => us.SubscribedTo)
                .Select(u => new SubscriptionPreviewVM
                {
                    Id = u.Id,
                    Name = u.Name,
                    JobPosition = u.JobPosition,
                    EnrollStudents = u.EnrollStudents,
                    CourseCnt = u.CoursesCnt
                })
                .ToListAsync();

            model.Subscriptions = subscriptions;
            
            User? curUser = null;

            if (identityUser != null)
            {
                curUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);
            }
            
            if (curUser != null)
            {
                ViewBag.CurrentUserId = curUser.Id;
                ViewBag.CurrentUserName = curUser.Name;
                ViewBag.CurrentJobPosition = curUser.JobPosition;
                ViewBag.CurrentSubscriptionsCnt = curUser.SubscriptionsCnt;
                ViewBag.CurrentEmail = curUser.Email;
            }
            
            return View(model);
        }
    }
}