using LmsAndOnlineCoursesMarketplace.Domain.Entities;
using LmsAndOnlineCoursesMarketplace.MVC.Models.LiveStream;
using LmsAndOnlineCoursesMarketplace.MVC.Models.OtherUserProfile;
using LmsAndOnlineCoursesMarketplace.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LmsAndOnlineCoursesMarketplace.MVC.Controllers;

public class LiveStreamController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;
    public LiveStreamController(UserManager<IdentityUser> userManager, 
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<IActionResult> Index(int? id)
    {
        var stream = await _context.LiveStreams.FindAsync(id);
        
        if (stream == null)
            return NotFound();
        
        stream.Views += 1;
        _context.LiveStreams.Update(stream);
        await _context.SaveChangesAsync();

        var identityUser = await _userManager.GetUserAsync(User);
        
        var currentUser = await _context.Users
            .Include(u => u.Subscriptions)
            .ThenInclude(us => us.SubscribedTo)
            .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);
        
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
        
        if (user == null)
        {
            return NotFound();
        }
        
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
        
        var viewModel = new LiveStreamVM
        {
            Id = stream.Id,
            UserId = stream.UserId,
            AuthorName = stream.User?.Name ?? "Unknown",
            EmbedUrl = stream.VideoLink,
            Views = stream.Views,
            LikesCnt = stream.LikesCnt,
            DislikesCnt = stream.DislikesCnt
        };
        
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
        return View(viewModel);
    }
    
    [HttpPost("GiveLike/{id}")]
    public async Task<IActionResult> GiveLike(int id)
    {
        var identityUser = await _userManager.GetUserAsync(User);
        if (identityUser == null) return Challenge();

        var currentUser = await _context.Users
            .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);

        if (currentUser == null) return NotFound();

        var stream = await _context.LiveStreams
            .FirstOrDefaultAsync(c => c.Id == id);

        if (stream == null) return NotFound();

        var reaction = await _context.LiveStreamReactions
            .FirstOrDefaultAsync(r => r.StreamId == id && r.UserId == currentUser.Id);

        if (reaction != null)
        {
            if (reaction.IsLike)
            {
                stream.LikesCnt -= 1;
                _context.LiveStreamReactions.Remove(reaction);
            }
            else if (reaction.IsDislike)
            {
                stream.DislikesCnt -= 1;
                stream.LikesCnt += 1;

                reaction.IsDislike = false;
                reaction.IsLike = true;

                _context.LiveStreamReactions.Update(reaction);
            }
        }
        else
        {
            stream.LikesCnt += 1;

            var newReaction = new LiveStreamReactions
            {
                StreamId = id,
                UserId = currentUser.Id,
                IsLike = true
            };

            await _context.LiveStreamReactions.AddAsync(newReaction);
        }

        _context.LiveStreams.Update(stream);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "LiveStream", new { id });
    }
    
    [HttpPost("GiveDislike/{id}")]
    public async Task<IActionResult> GiveDislike(int id)
    {
        var identityUser = await _userManager.GetUserAsync(User);
        if (identityUser == null) return Challenge();

        var currentUser = await _context.Users
            .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);

        if (currentUser == null) return NotFound();

        var stream = await _context.LiveStreams
            .FirstOrDefaultAsync(c => c.Id == id);

        if (stream == null) return NotFound();

        var reaction = await _context.LiveStreamReactions
            .FirstOrDefaultAsync(r => r.StreamId == id && r.UserId == currentUser.Id);

        if (reaction != null)
        {
            if (reaction.IsDislike)
            {
                stream.DislikesCnt -= 1;
                _context.LiveStreamReactions.Remove(reaction);
            }
            else if (reaction.IsLike)
            {
                stream.LikesCnt -= 1;
                stream.DislikesCnt += 1;

                reaction.IsLike = false;
                reaction.IsDislike = true;

                _context.LiveStreams.Update(stream);
                _context.LiveStreamReactions.Update(reaction);
            }
        }
        else
        {
            stream.DislikesCnt += 1;

            var newReaction = new LiveStreamReactions
            {
                StreamId = id,
                UserId = currentUser.Id,
                IsDislike = true
            };

            await _context.LiveStreamReactions.AddAsync(newReaction);
            _context.LiveStreams.Update(stream);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Index", new { id });
    }
}