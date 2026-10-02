using MediatR;

namespace CarePrideSystem.Application.Features.Auth.AuthCommands
{
    public class ApproveUserCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}