using LmsAndOnlineCoursesMarketplace.Domain.Entities;
using LmsAndOnlineCoursesMarketplace.MVC.Models.OtherUserProfile;
using LmsAndOnlineCoursesMarketplace.Persistence.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LmsAndOnlineCoursesMarketplace.MVC.Controllers;

public class LiveStreamsController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;
    public LiveStreamsController(UserManager<IdentityUser> userManager, 
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }
    
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var identityUser = await _userManager.GetUserAsync(User);
        User? curUser = null;
        
        var currentUser = await _context.Users
            .Include(u => u.Subscriptions)
            .ThenInclude(us => us.SubscribedTo)
            .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);
        
        var allStreamers = await _context.LiveStreams
            .Include(s => s.User)
            //.Where(s => s.UserId != currentUser.Id) // Исключаем себя
            .Select(s => new
            {
                Id = s.Id,
                Name = s.User.Name,
                VideoLink = s.VideoLink,
                Avatar = s.UserId
            })
            .ToListAsync();

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
            ViewBag.CurrentEnrollStudents = curUser.EnrollStudents;
            ViewBag.CurrentCoursesCnt = curUser.CoursesCnt;
            ViewBag.CurrentEmail = curUser.Email;
            ViewBag.Subscriptions = currentUser.Subscriptions?
                .Select(us => new SubscriptionPreviewVM()
                {
                    Id = us.SubscribedToId,
                    Name = us.SubscribedTo?.Name ?? "Unknown",
                }).ToList() ?? new List<SubscriptionPreviewVM>();
            ViewBag.AllStreamers = allStreamers;
        }
        return View();
    }
}