using Microsoft.AspNetCore.Mvc;

namespace LmsAndOnlineCoursesMarketplace.MVC.Controllers;

public class LoginController : Controller
{
    public LoginController()
    {
        
    }

    public IActionResult Index()
    {
        return View();
    }
}