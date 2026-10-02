using MediatR;
using CarePrideSystem.Application.DTOs.Auth;

namespace CarePrideSystem.Application.Features.Auth.AuthCommands
{
    public class LoginCommand : IRequest<TokenResponseDto>
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
