namespace LmsAndOnlineCoursesMarketplace.Application.Interfaces.Services;

public interface IAuthService
{
    Task<bool> RegisterAsync(string fullName, string email, string password);
    
    Task<bool> LoginAsync(string email, string password);
}