using MediatR;

namespace LmsAndOnlineCoursesMarketplace.Application.Features.Auth.Login;

public class LoginCommand : IRequest<bool>
{
    public string Email { get; init; }
    public string Password { get; init; }
}