using LmsAndOnlineCoursesMarketplace.Domain.Entities;
using LmsAndOnlineCoursesMarketplace.MVC.Models.Course;
using LmsAndOnlineCoursesMarketplace.MVC.Models.OtherUserProfile;
using LmsAndOnlineCoursesMarketplace.MVC.Models.Profile;
using LmsAndOnlineCoursesMarketplace.Persistence.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LmsAndOnlineCoursesMarketplace.MVC.Areas.Admin.Controllers;

[Authorize]
[Area("Admin")]
public class DashboardController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;

    public DashboardController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var identityUser = await _userManager.GetUserAsync(User);
        User? curUser = null;

        if (identityUser != null)
        {
            curUser = await _context.Users
                .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);
        }
        
        var currentUser = await _context.Users
            .Include(u => u.Courses)
            .Include(u => u.Subscriptions)
            .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);

        var model = new ProfileVM
        {
            CreatedCourses = currentUser?.Courses?
                .Select(c => new CourseVM
                {
                    Id = c.Id,
                    Title = c.Title,
                    ImageLink = c.ImageLink,
                    Price = c.Price,
                    Category = c.Category,
                    Language = c.Language,
                    UserId = c.UserId,
                    Views = c.Views,
                    LikesCnt = c.LikesCnt,
                    DislikesCnt = c.DislikesCnt
                })
                .ToList() ?? new List<CourseVM>(),
        };
            
        if (curUser != null)
        {
            ViewBag.CurrentUserId = curUser.Id;
            ViewBag.CurrentUserName = curUser.Name;
            ViewBag.CurrentJobPosition = curUser.JobPosition;
            ViewBag.CurrentSubscriptionsCnt = curUser.SubscriptionsCnt;
            ViewBag.CurrentEnrollStudents = curUser.EnrollStudents;
            ViewBag.CurrentCoursesCnt = curUser.CoursesCnt;
            ViewBag.CurrentEmail = curUser.Email;
            ViewBag.CurrentUserBalance = curUser.Balance;
            ViewBag.TotalCourses = currentUser?.Courses.Count ?? 0;
            ViewBag.Subscriptions = currentUser?.Subscriptions?
                .Select(us => new SubscriptionPreviewVM()
                {
                    Id = us.SubscribedToId,
                    Name = us.SubscribedTo?.Name ?? "Unknown",
                }).ToList() ?? new List<SubscriptionPreviewVM>();
            ViewBag.EnrollSubscriptions = currentUser?.Subscriptions?.Count ?? 0;
        }
        
        return View(model);
    }
}