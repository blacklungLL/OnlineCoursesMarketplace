using MediatR;

namespace LmsAndOnlineCoursesMarketplace.Application.Features.Auth.Register;

public class RegisterCommand: IRequest<bool>
{
    public string Email { get; init; }
    public string Password { get; init; }
}