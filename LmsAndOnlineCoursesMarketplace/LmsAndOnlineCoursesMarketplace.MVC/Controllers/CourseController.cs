using Microsoft.AspNetCore.Mvc;

namespace LmsAndOnlineCoursesMarketplace.MVC.Controllers;

public class CourseController : Controller
{
    public CourseController()
    {
        
    }

    public IActionResult Index()
    {
        return View();
    }
}