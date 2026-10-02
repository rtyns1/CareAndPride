using MediatR;

namespace CarePrideSystem.Application.Features.Auth.AuthCommands
{
    public class RejectUserCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}