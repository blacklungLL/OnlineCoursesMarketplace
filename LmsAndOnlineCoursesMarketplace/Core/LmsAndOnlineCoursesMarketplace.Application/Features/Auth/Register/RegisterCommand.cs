using MediatR;

namespace LmsAndOnlineCoursesMarketplace.Application.Features.Auth.Register;

public class RegisterCommand: IRequest<bool>
{
    public string FullName { get; set; }
    public string Email { get; init; }
    public string Password { get; init; }
}