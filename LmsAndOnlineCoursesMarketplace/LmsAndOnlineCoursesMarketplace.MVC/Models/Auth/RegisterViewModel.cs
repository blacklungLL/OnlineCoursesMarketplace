namespace LmsAndOnlineCoursesMarketplace.MVC.Models.Auth;

public class RegisterViewModel
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required bool ImInForEmails { get; set; }
}