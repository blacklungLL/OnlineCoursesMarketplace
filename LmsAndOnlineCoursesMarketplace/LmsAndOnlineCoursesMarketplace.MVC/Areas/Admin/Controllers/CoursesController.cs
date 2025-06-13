using LmsAndOnlineCoursesMarketplace.Domain.Entities;
using LmsAndOnlineCoursesMarketplace.MVC.Models.Course;
using LmsAndOnlineCoursesMarketplace.MVC.Models.OtherUserProfile;
using LmsAndOnlineCoursesMarketplace.MVC.Models.Profile;
using LmsAndOnlineCoursesMarketplace.MVC.Models.ShoppingCart;
using LmsAndOnlineCoursesMarketplace.Persistence.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LmsAndOnlineCoursesMarketplace.MVC.Areas.Admin.Controllers;

[Authorize]
[Area("Admin")]
public class CoursesController: Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;
    
    public CoursesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
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
        
        var purchased = await _context.UserCoursePurchases
            .Include(up => up.Course)
            .ThenInclude(c => c.User)
            .Where(up => up.UserId == currentUser.Id)
            .Select(up => up.Course)
            .ToListAsync();

        model.PurchasedCourses = purchased.Select(c => new CourseSummaryVM
        {
            Id = c.Id,
            AuthorId = c.User.Id,
            Title = c.Title,
            ImageLink = c.ImageLink,
            Category = c.Category,
            Language = c.Language,
            Duration = c.Duration,
            Views = c.Views,
            Price = c.Price,
            AuthorName = c.User?.Name ?? "Unknown"
        }).ToList();
            
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
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCourse(int courseId)
    {
        var identityUser = await _userManager.GetUserAsync(User);
        if (identityUser == null) return Challenge();

        var currentUser = await _context.Users
            .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);

        if (currentUser == null) return NotFound();

        var course = await _context.Courses
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null)
            return NotFound();

        if (course.UserId != currentUser.Id)
        {
            TempData["Message"] = "You cannot delete foreign course";
            return RedirectToAction("Index");
        }

        var purchases = await _context.UserCoursePurchases
            .Include(up => up.User)
            .Where(up => up.CourseId == courseId)
            .ToListAsync();

        var purchasers = purchases.Select(p => p.User).ToList();

        foreach (var purchaser in purchasers)
        {
            purchaser.Balance += course.Price;
            _context.Users.Update(purchaser);
        }

        currentUser.Balance -= course.Price * purchasers.Count;
        _context.Users.Update(currentUser);

        _context.UserCoursePurchases.RemoveRange(purchases);

        _context.Courses.Remove(course);

        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Course '{course.Title}' was deleted. {purchasers.Count} users got refund";

        return RedirectToAction("Index", "Courses");
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RefundCourse(int courseId)
    {
        var identityUser = await _userManager.GetUserAsync(User);
        if (identityUser == null) return Challenge();

        var currentUser = await _context.Users
            .Include(u => u.Courses)
            .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);

        if (currentUser == null) return NotFound();

        var course = await _context.Courses
            .FirstOrDefaultAsync(c => c.Id == courseId);

        if (course == null) return NotFound();

        var purchase = await _context.UserCoursePurchases
            .FirstOrDefaultAsync(up => up.UserId == currentUser.Id && up.CourseId == courseId);

        if (purchase == null)
        {
            TempData["Message"] = "You did not buy this course.";
            return RedirectToAction("Index");
        }

        currentUser.Balance += course.Price;
        _context.Users.Update(currentUser);

        var author = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == course.UserId);

        if (author != null)
        {
            author.Balance -= course.Price;
            _context.Users.Update(author);
        }

        _context.UserCoursePurchases.Remove(purchase);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Course '{course.Title}' Was deleted. You were returned {course.Price}$";
        return RedirectToAction("Index", "Courses");
    }
}