using Microsoft.AspNetCore.Mvc;

namespace LmsAndOnlineCoursesMarketplace.MVC.Controllers;

public class SettingsController : Controller
{
    public SettingsController()
    {
        
    }

    public IActionResult Index()
    {
        return View();
    }
}