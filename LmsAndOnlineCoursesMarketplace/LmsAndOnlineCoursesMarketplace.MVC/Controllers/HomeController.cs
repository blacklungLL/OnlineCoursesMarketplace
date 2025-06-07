using System.Diagnostics;
using LmsAndOnlineCoursesMarketplace.Application.Features.Courses.Queries;
using Microsoft.AspNetCore.Mvc;
using LmsAndOnlineCoursesMarketplace.MVC.Models;
using LmsAndOnlineCoursesMarketplace.MVC.Models.Home;
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
        var courseId = 0;
        var query = new GetByCourseIdQuery(courseId);
        var courses = await _mediator.Send(query);
        var viewModel = new HomeVM
        {
            FeaturedCourses = courses,
            AllCourses = courses,
        };
        
        var identityUser = await _userManager.GetUserAsync(User);
    
        if (identityUser != null)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.IdentityUserId == identityUser.Id);

            if (user != null)
            {
                ViewBag.UserId = user.Id;
                ViewBag.UserName = user.Name;
                ViewBag.JobPosition = user.JobPosition;
                ViewBag.EnrollStudents = user.EnrollStudents;
                ViewBag.CoursesCnt = user.CoursesCnt;
                ViewBag.UserEmail = user.Email;
            }
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