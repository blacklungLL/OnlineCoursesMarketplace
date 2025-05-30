using LmsAndOnlineCoursesMarketplace.Application.Interfaces.Services;
using MediatR;

namespace LmsAndOnlineCoursesMarketplace.Application.Features.Auth.Login;

public class LoginCommandHandler(IAuthService _authService) : IRequestHandler<LoginCommand, bool>
{
    public async Task<bool> Handle(LoginCommand command, CancellationToken cancellationToken) 
        => await _authService.LoginAsync(command.Email, command.Password);
}