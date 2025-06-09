using LmsAndOnlineCoursesMarketplace.Application.Features.Courses.Queries;
using LmsAndOnlineCoursesMarketplace.Domain.Entities;
using LmsAndOnlineCoursesMarketplace.MVC.Models.Course;
using LmsAndOnlineCoursesMarketplace.MVC.Models.OtherUserProfile;
using LmsAndOnlineCoursesMarketplace.Persistence.Contexts;
using MediatR;
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

    public async Task<IActionResult> Index(int id)
    {
        var query = new GetByCourseIdQuery(id);
        var courses = await _mediator.Send(query);
        var course = courses.FirstOrDefault();
        
        if (course == null)
            return NotFound();

        var viewModel = new CourseVM
        {
            Id = course.Id,
            Title = course.Title,
            ShortDescription = course.ShortDescription,
            UserId = course.UserId,
            AuthorName = course.AuthorName ?? "Unknown",
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
}