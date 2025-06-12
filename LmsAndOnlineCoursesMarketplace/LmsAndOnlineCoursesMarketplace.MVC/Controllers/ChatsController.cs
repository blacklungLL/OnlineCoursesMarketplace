using LmsAndOnlineCoursesMarketplace.Domain.Entities;
using LmsAndOnlineCoursesMarketplace.MVC.Models.Chat;
using LmsAndOnlineCoursesMarketplace.MVC.Models.OtherUserProfile;
using LmsAndOnlineCoursesMarketplace.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LmsAndOnlineCoursesMarketplace.MVC.Controllers;

public class ChatsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public ChatsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? userId)
    {
        var identityUser = await _userManager.GetUserAsync(User);
        var currentUser = await _context.Users
            .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);

        if (currentUser == null)
            return Challenge();

        var messages = await _context.ChatMessages
            .Where(m => m.SenderId == currentUser.Id || m.RecipientId == currentUser.Id)
            .Include(m => m.Sender)
            .Include(m => m.Recipient)
            .ToListAsync();

        var otherUsers = messages
            .Select(m => 
                m.SenderId == currentUser.Id ? m.Recipient : m.Sender)
            .Where(u => u != null)
            .DistinctBy(u => u.Id)
            .ToList();

        var model = new ChatVM
        {
            Users = otherUsers.Select(u => new ChatUserVM
            {
                Id = u.Id,
                Name = u.Name,
                AvatarUrl = $"~/assets/images/left-imgs/img-{u.Id}.jpg"
            }).ToList(),
            SelectedUserId = userId,
            SelectedUserName = userId.HasValue 
                ? await _context.Users.Where(u => u.Id == userId.Value).Select(u => u.Name).FirstOrDefaultAsync()
                : null
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
            ViewBag.CurrentEnrollStudents = curUser.EnrollStudents;
            ViewBag.CurrentCoursesCnt = curUser.CoursesCnt;
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
}