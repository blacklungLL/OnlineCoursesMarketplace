using Microsoft.AspNetCore.Mvc;

namespace LmsAndOnlineCoursesMarketplace.MVC.Controllers;

public class LiveStreamController : Controller
{
    public LiveStreamController()
    {
        
    }

    public IActionResult Index()
    {
        return View();
    }
}