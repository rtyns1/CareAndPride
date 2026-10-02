using MediatR;
using CarePrideSystem.Domain.Enums;

namespace CarePrideSystem.Application.Features.Auth.AuthCommands
{
    public class CreateUserCommand : IRequest<Guid>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public UserRole Role { get; set; }
        public string Password { get; set; }
    }
}