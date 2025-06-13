using LmsAndOnlineCoursesMarketplace.Application.Features.Auth.Login;
using LmsAndOnlineCoursesMarketplace.Application.Features.Auth.Register;
using LmsAndOnlineCoursesMarketplace.MVC.Models.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LmsAndOnlineCoursesMarketplace.MVC.Controllers;

public class AuthController : Controller
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public ActionResult Login()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        var command = new RegisterCommand
        {
            FullName = model.FullName,
            Email = model.Email,
            Password = model.Password
        };
        var IsSuccess = await _mediator.Send(command);

        if (IsSuccess)
            return RedirectToAction("Index", "Home");

        //ModelState.AddModelError(string.Empty, IsSuccess.Error);
        return RedirectToAction("Index", "Registration");
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
    {
        var command = new LoginCommand
        {
            Email = model.Email,
            Password = model.Password,
        };

        var IsSuccess = await _mediator.Send(command);

        if (IsSuccess)
        {
            returnUrl ??= Url.Content("~/");
            return LocalRedirect(returnUrl);
        }

        ModelState.AddModelError(string.Empty, "Неверный логин или пароль.");
        return View(model);
    }
}