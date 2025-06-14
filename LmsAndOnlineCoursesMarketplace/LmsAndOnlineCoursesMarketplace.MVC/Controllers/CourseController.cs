using LmsAndOnlineCoursesMarketplace.Domain.Entities;
using LmsAndOnlineCoursesMarketplace.MVC.Models.Course;
using LmsAndOnlineCoursesMarketplace.MVC.Models.OtherUserProfile;
using LmsAndOnlineCoursesMarketplace.Persistence.Contexts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LmsAndOnlineCoursesMarketplace.MVC.Controllers;

public class CourseController : Controller
{
    private readonly IMediator _mediator;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;

    public CourseController(IMediator mediator, UserManager<IdentityUser> userManager, ApplicationDbContext context)
    {
        _mediator = mediator;
        _userManager = userManager;
        _context = context;
    }
    
    [Authorize]
    public async Task<IActionResult> Index(int id)
    {
        var course = await _context.Courses.FindAsync(id);
        
        if (course == null)
            return NotFound();
        
        course.Views += 1;
        _context.Courses.Update(course);
        await _context.SaveChangesAsync();

        var courseAuthor = await _context.Courses
            .Include(s => s.User)
            .FirstOrDefaultAsync();

        var viewModel = new CourseVM
        {
            Id = course.Id,
            Title = course.Title,
            ShortDescription = course.ShortDescription,
            UserId = course.UserId,
            AuthorName = courseAuthor?.User.Name ?? "Unknown",
            ImageLink = course.ImageLink,
            Rating = course.Rating,
            RatingsCnt = course.RatingsCnt,
            Language = course.Language,
            LastUpdate = course.LastUpdate,
            Views = course.Views,
            LikesCnt = course.LikesCnt,
            DislikesCnt = course.DislikesCnt,
            SharedCnt = course.SharedCnt,
            Requirements = course.Requirements,
            Description = course.Description,
            Duration = course.Duration,
            Category = course.Category,
            Price = course.Price
        };
        
        var identityUser = await _userManager.GetUserAsync(User);
        
        var currentUser = await _context.Users
            .Include(u => u.Subscriptions)
            .ThenInclude(us => us.SubscribedTo)
            .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);
    
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
            ViewBag.Subscriptions = currentUser.Subscriptions?
                .Select(us => new SubscriptionPreviewVM()
                {
                    Id = us.SubscribedToId,
                    Name = us.SubscribedTo?.Name ?? "Unknown",
                }).ToList() ?? new List<SubscriptionPreviewVM>();
        }

        return View(viewModel);
    }
    
    [HttpPost("Like/{id}")]
    public async Task<IActionResult> Like(int id)
    {
        var identityUser = await _userManager.GetUserAsync(User);
        if (identityUser == null) return Challenge();

        var currentUser = await _context.Users
            .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);

        if (currentUser == null) return NotFound();

        var course = await _context.Courses
            .FirstOrDefaultAsync(c => c.Id == id);

        if (course == null) return NotFound();

        var reaction = await _context.CourseReactions
            .FirstOrDefaultAsync(r => r.CourseId == id && r.UserId == currentUser.Id);

        if (reaction != null)
        {
            if (reaction.IsLike)
            {
                course.LikesCnt -= 1;
                _context.CourseReactions.Remove(reaction);
            }
            else if (reaction.IsDislike)
            {
                course.DislikesCnt -= 1;
                course.LikesCnt += 1;

                reaction.IsDislike = false;
                reaction.IsLike = true;

                _context.CourseReactions.Update(reaction);
            }
        }
        else
        {
            course.LikesCnt += 1;

            var newReaction = new CourseReaction
            {
                CourseId = id,
                UserId = currentUser.Id,
                IsLike = true
            };

            await _context.CourseReactions.AddAsync(newReaction);
        }

        _context.Courses.Update(course);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Course", new { id });
    }
    
    [HttpPost("Dislike/{id}")]
    public async Task<IActionResult> Dislike(int id)
    {
        var identityUser = await _userManager.GetUserAsync(User);
        if (identityUser == null) return Challenge();

        var currentUser = await _context.Users
            .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);

        if (currentUser == null) return NotFound();

        var course = await _context.Courses
            .FirstOrDefaultAsync(c => c.Id == id);

        if (course == null) return NotFound();

        var reaction = await _context.CourseReactions
            .FirstOrDefaultAsync(r => r.CourseId == id && r.UserId == currentUser.Id);

        if (reaction != null)
        {
            if (reaction.IsDislike)
            {
                course.DislikesCnt -= 1;
                _context.CourseReactions.Remove(reaction);
            }
            else if (reaction.IsLike)
            {
                course.LikesCnt -= 1;
                course.DislikesCnt += 1;

                reaction.IsLike = false;
                reaction.IsDislike = true;

                _context.Courses.Update(course);
                _context.CourseReactions.Update(reaction);
            }
        }
        else
        {
            course.DislikesCnt += 1;

            var newReaction = new CourseReaction
            {
                CourseId = id,
                UserId = currentUser.Id,
                IsDislike = true
            };

            await _context.CourseReactions.AddAsync(newReaction);
            _context.Courses.Update(course);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Index", new { id });
    }
}