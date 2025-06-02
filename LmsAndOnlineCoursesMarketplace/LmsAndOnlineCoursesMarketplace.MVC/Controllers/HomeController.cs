using System.Diagnostics;
using LmsAndOnlineCoursesMarketplace.Application.Features.Courses.Queries;
using LmsAndOnlineCoursesMarketplace.Application.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;
using LmsAndOnlineCoursesMarketplace.MVC.Models;
using LmsAndOnlineCoursesMarketplace.MVC.Models.Home;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace LmsAndOnlineCoursesMarketplace.MVC.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMediator _mediator;
    public HomeController(ILogger<HomeController> logger ,IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
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
            AllCourses = courses
        };
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