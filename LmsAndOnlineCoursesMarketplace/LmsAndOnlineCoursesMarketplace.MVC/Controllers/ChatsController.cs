using Microsoft.AspNetCore.Mvc;

namespace LmsAndOnlineCoursesMarketplace.MVC.Controllers;

public class ChatsController : Controller
{
    public ChatsController()
    {
        
    }

    public IActionResult Index()
    {
        return View();
    }
}