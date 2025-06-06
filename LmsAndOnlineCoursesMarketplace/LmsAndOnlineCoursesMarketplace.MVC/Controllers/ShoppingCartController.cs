using Microsoft.AspNetCore.Mvc;

namespace LmsAndOnlineCoursesMarketplace.MVC.Controllers;

public class ShoppingCartController: Controller
{
    public ShoppingCartController()
    {
        
    }

    public IActionResult Index()
    {
        return View();
    }
}