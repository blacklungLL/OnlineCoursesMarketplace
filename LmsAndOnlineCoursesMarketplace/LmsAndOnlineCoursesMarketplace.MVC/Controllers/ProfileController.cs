using Microsoft.AspNetCore.Mvc;

namespace LmsAndOnlineCoursesMarketplace.MVC.Controllers;

public class ProfileController : Controller
{
    public ProfileController()
    {
        
    }

    public IActionResult Index()
    {
        return View();
    }
}