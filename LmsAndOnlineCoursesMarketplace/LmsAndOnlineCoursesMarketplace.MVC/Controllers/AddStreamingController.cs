using LmsAndOnlineCoursesMarketplace.Domain.Entities;
using LmsAndOnlineCoursesMarketplace.MVC.Models.LiveStream;
using LmsAndOnlineCoursesMarketplace.MVC.Models.OtherUserProfile;
using LmsAndOnlineCoursesMarketplace.Persistence.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LmsAndOnlineCoursesMarketplace.MVC.Controllers;

public class AddStreamingController: Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;
    public AddStreamingController(UserManager<IdentityUser> userManager, 
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }
    
    [Authorize]
    public async Task<IActionResult> Index()
    {
        
        var identityUser = await _userManager.GetUserAsync(User);
        
        var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);
        if (currentUser == null) return NotFound();
        
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

        var model = new LiveStreamVM
        {
            UserId = currentUser.Id,
            EmbedUrl = String.Empty
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
            ViewBag.Subscriptions = currentProfile?.Subscriptions?
                .Select(us => new SubscriptionPreviewVM()
                {
                    Id = us.SubscribedToId,
                    Name = us.SubscribedTo?.Name ?? "Unknown",
                }).ToList() ?? new List<SubscriptionPreviewVM>();
        }
        return View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> Index(LiveStreamVM model)
    {
        var identityUser = await _userManager.GetUserAsync(User);
        if (identityUser == null) return Challenge();

        var currentUser = await _context.Users
            .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);
        if (currentUser == null) return NotFound();

        string videoEmbedUrl;

        var videoId = TryExtractYouTubeVideoId(model.VideoSource);

        if (!string.IsNullOrEmpty(videoId))
        {
            videoEmbedUrl = $"https://www.youtube.com/embed/{videoId}"; 
        }
        else
        {
            videoEmbedUrl = model.VideoSource.Trim();
        }

        var existingStream = await _context.LiveStreams
            .FirstOrDefaultAsync(s => s.UserId == currentUser.Id);

        if (existingStream != null)
        {
            existingStream.VideoLink = videoEmbedUrl;
            _context.Update(existingStream);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "LiveStream", new { id = existingStream.Id });
        }

        var stream = new LiveStream
        {
            UserId = currentUser.Id,
            VideoLink = videoEmbedUrl
        };

        _context.LiveStreams.Add(stream);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "LiveStream", new { id = stream.Id });
    }
    
    private string TryExtractYouTubeVideoId(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;

        try
        {
            var uri = new Uri(input.Trim());

            if (uri.Host == "www.youtube.com" || uri.Host == "youtube.com")
            {
                // Извлекаем ID из параметров
                var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
                return query["v"];
            }

            return input;
        }
        catch (UriFormatException)
        {
            return input;
        }
    }
}