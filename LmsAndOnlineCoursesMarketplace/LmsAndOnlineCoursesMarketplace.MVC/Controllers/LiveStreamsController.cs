using Microsoft.AspNetCore.Mvc;

namespace LmsAndOnlineCoursesMarketplace.MVC.Controllers;

public class LiveStreamsController : Controller
{
    public LiveStreamsController()
    {
        
    }

    public IActionResult Index()
    {
        return View();
    }
}