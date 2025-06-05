using LmsAndOnlineCoursesMarketplace.Application.Interfaces.Services;
using LmsAndOnlineCoursesMarketplace.Domain.Entities;
using LmsAndOnlineCoursesMarketplace.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LmsAndOnlineCoursesMarketplace.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IEmailSender _emailSender;
    private readonly ApplicationDbContext _context;
 
    public AuthService(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        IEmailSender emailSender,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
        _context = context;
    }
 
    public async Task<bool> RegisterAsync(string fullName, string email, string password)
    {
        var identityUser = new IdentityUser { Email = email, UserName = email };
        var result = await _userManager.CreateAsync(identityUser, password);

        if (!result.Succeeded)
            return false;
 
        await _signInManager.SignInAsync(identityUser, isPersistent: false);
        
        var user = new User
        {
            Name = fullName,
            Email = email,
            JobPosition = "Не указано",
            Description = "Не указано",
            EnrollStudents = 0,
            CoursesCnt = 0,
            ReviewsCnt = 0,
            SubscriptionsCnt = 0,
            IdentityUserId = identityUser.Id
        };
        
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        
        // Отправка письма
        await _emailSender.SendAsync(email, "Регистрация", $"Ваш логин: {email}\nВаш пароль: {password}");
        
        return true;
    }
 
    public async Task<bool> LoginAsync(string email, string password)
    {
        var identityUser = await _userManager.FindByEmailAsync(email);
        if (identityUser == null)
            return false;
        
        await _signInManager.PasswordSignInAsync(identityUser, password, false, false);
        
        return true;
    }
}