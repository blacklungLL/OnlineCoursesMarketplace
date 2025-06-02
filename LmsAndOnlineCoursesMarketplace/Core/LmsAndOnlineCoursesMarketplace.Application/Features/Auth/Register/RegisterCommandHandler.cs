using LmsAndOnlineCoursesMarketplace.Application.Interfaces.Services;
using MediatR;

namespace LmsAndOnlineCoursesMarketplace.Application.Features.Auth.Register;

public class RegisterCommandHandler(IAuthService _authService) : IRequestHandler<RegisterCommand, bool>
{
    public async Task<bool> Handle(RegisterCommand command, CancellationToken cancellationToken) 
        => await _authService.RegisterAsync(command.FullName, command.Email, command.Password);
}