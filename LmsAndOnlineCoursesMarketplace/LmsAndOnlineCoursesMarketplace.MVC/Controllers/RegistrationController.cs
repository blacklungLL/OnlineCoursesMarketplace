using Microsoft.AspNetCore.Mvc;

namespace LmsAndOnlineCoursesMarketplace.MVC.Controllers;

public class RegistrationController : Controller
{
    public RegistrationController()
    {
        
    }

    public IActionResult Index()
    {
        return View();
    }
}