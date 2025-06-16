using LmsAndOnlineCoursesMarketplace.Application.Interfaces.Services;
using LmsAndOnlineCoursesMarketplace.Domain.Entities;
using LmsAndOnlineCoursesMarketplace.MVC.Models.Course;
using LmsAndOnlineCoursesMarketplace.MVC.Models.OtherUserProfile;
using LmsAndOnlineCoursesMarketplace.Persistence.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LmsAndOnlineCoursesMarketplace.MVC.Areas.Admin.Controllers;

[Authorize]
[Area("Admin")]
public class CreateCourseController: Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly IS3Service _s3Service;
    
    public CreateCourseController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IS3Service s3Service)
    {
        _userManager = userManager;
        _context = context;
        _s3Service = s3Service;
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

        
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCourseVM model, IFormFile? image)
    {
        var identityUser = await _userManager.GetUserAsync(User);
        if (identityUser == null) return Challenge();

        var currentUser = await _context.Users
            .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);

        if (currentUser == null) return NotFound();

        if (!ModelState.IsValid)
        {
            foreach (var key in ModelState.Keys)
            {
                var state = ModelState[key];
                Console.WriteLine($"Error in the field '{key}': {string.Join(", ", state.Errors.Select(e => e.ErrorMessage))}");
            }

            TempData["Message"] = "You have an error in one of the fields. Please try again.";
            return RedirectToAction("Index", "CreateCourse");
        }
        
        string imageLink = "/assets/images/courses/placeholder.jpg";
        
        if (image != null && image.Length > 0)
        {
            using var stream = image.OpenReadStream();
            var key = $"courses/{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            imageLink = await _s3Service.UploadFileAsync(key, stream, image.ContentType);
        }
        
        var bucketExists = await _s3Service.DoesBucketExistAsync();
        if (!bucketExists)
        {
            TempData["Message"] = "Bucket does not exist.";
            return RedirectToAction("Index", "CreateCourse");
        }

        var newCourse = new Course
        {
            Title = model.Title,
            ShortDescription = model.ShortDescription,
            Description = model.Description,
            Requirements = model.Requirements,
            Duration = model.Duration,
            Category = model.Category,
            Language = model.Language,
            Price = model.Price,

            UserId = currentUser.Id,
            ImageLink = imageLink,
            CreatedBy = currentUser.Id,
            UpdatedBy = currentUser.Id,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            
            LastUpdate = 2024,
            Rating = 4,
            RatingsCnt = 2544,
            Views = 0,
            LikesCnt = 0,
            DislikesCnt = 0,
            SharedCnt = 0
        };

        await _context.Courses.AddAsync(newCourse);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Course '{newCourse.Title}' successfully created.";
        return RedirectToAction("Index", "Courses");
    }
}