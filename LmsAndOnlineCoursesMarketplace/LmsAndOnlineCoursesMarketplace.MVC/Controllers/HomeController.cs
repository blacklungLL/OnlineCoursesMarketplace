using System.Diagnostics;
using LmsAndOnlineCoursesMarketplace.Application.Features.Courses.Queries;
using LmsAndOnlineCoursesMarketplace.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using LmsAndOnlineCoursesMarketplace.MVC.Models;
using LmsAndOnlineCoursesMarketplace.MVC.Models.Home;
using LmsAndOnlineCoursesMarketplace.MVC.Models.OtherUserProfile;
using LmsAndOnlineCoursesMarketplace.Persistence.Contexts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LmsAndOnlineCoursesMarketplace.MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMediator _mediator;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly ApplicationDbContext _context;
    public HomeController(ILogger<HomeController> logger,
            IMediator mediator, 
            UserManager<IdentityUser> userManager, 
            ApplicationDbContext context)
    {
        _logger = logger;
        _mediator = mediator;
        _userManager = userManager;
        _context = context;
    }
    
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var identityUser = await _userManager.GetUserAsync(User);
        
        var courseId = 0;
        var query = new GetByCourseIdQuery(courseId);
        var courses = await _mediator.Send(query);
        
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
        
        var viewModel = new HomeVM
        {
            FeaturedCourses = courses,
            AllCourses = courses,
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

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}