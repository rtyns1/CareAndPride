using MediatR;

namespace CarePrideSystem.Application.Features.Auth.AuthCommands
{
    public class UpdateUserCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}