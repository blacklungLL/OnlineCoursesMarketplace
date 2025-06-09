using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LmsAndOnlineCoursesMarketplace.Persistence.Contexts;
using LmsAndOnlineCoursesMarketplace.Domain.Entities;
using LmsAndOnlineCoursesMarketplace.MVC.Models.Course;
using LmsAndOnlineCoursesMarketplace.MVC.Models.OtherUserProfile;
using LmsAndOnlineCoursesMarketplace.MVC.Models.Profile;
using Microsoft.AspNetCore.Identity;

namespace LmsAndOnlineCoursesMarketplace.MVC.Controllers
{
    public class OtherUserProfileController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        public OtherUserProfileController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("OtherUserProfile/{id}")]
        public async Task<IActionResult> Index(int id)
        {
            var user = await _context.Users
                .Include(u => u.Courses)
                .Include(u => u.Subscribers)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var identityUser = await _userManager.GetUserAsync(User);
            int currentUserId = -1;

            if (identityUser != null)
            {
                var currentUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);

                currentUserId = currentUser?.Id ?? -1;
            }

            bool isSubscribed = false;
            if (currentUserId != -1)
            {
                isSubscribed = await _context.UserSubscriptions
                    .AnyAsync(us => us.SubscriberId == currentUserId && us.SubscribedToId == user.Id);
            }

            var model = new ProfileVM
            {
                Id = user.Id,
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email,
                JobPosition = user.JobPosition,
                Description = user.Description,

                EnrollStudents = user.EnrollStudents,
                CoursesCnt = user.Courses.Count,
                ReviewsCnt = user.ReviewsCnt,
                SubscriptionsCnt = user.SubscriptionsCnt,

                IsSubscribed = isSubscribed,
                CurrentUserId = currentUserId,

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
        
        [HttpPost("OtherUserProfile/Subscribe/{userId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Subscribe(int userId)
        {
            var identityUser = await _userManager.GetUserAsync(User);
            if (identityUser == null) return Challenge();

            var currentUser = await _context.Users
                .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);

            if (currentUser == null) return NotFound();

            var targetUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (targetUser == null) return NotFound();
            
            if (currentUser.Id == userId)
                return RedirectToAction("Index", "OtherUserProfile", new { id = userId });
            
            var alreadySubscribed = await _context.UserSubscriptions
                .AnyAsync(us => us.SubscriberId == currentUser.Id && us.SubscribedToId == targetUser.Id);

            if (alreadySubscribed)
                return RedirectToAction("Index", new { id = userId });
            
            var subscription = new UserSubscription
            {
                SubscriberId = currentUser.Id,
                SubscribedToId = targetUser.Id
            };

            await _context.UserSubscriptions.AddAsync(subscription);

            targetUser.SubscriptionsCnt += 1;

            _context.Users.Update(targetUser);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "OtherUserProfile", new { id = userId });
        }
        
        [HttpPost("OtherUserProfile/Unsubscribe/{userId}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unsubscribe(int userId)
        {
            var identityUser = await _userManager.GetUserAsync(User);
            if (identityUser == null) return Challenge();

            var currentUser = await _context.Users
                .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);

            if (currentUser == null) return NotFound();

            var targetUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (targetUser == null) return NotFound();

            var subscription = await _context.UserSubscriptions
                .FirstOrDefaultAsync(us => us.SubscriberId == currentUser.Id && us.SubscribedToId == userId);

            if (subscription != null)
            {
                _context.UserSubscriptions.Remove(subscription);
                targetUser.SubscriptionsCnt -= 1;
                _context.Users.Update(targetUser);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "OtherUserProfile", new { id = userId });
        }
    }
}