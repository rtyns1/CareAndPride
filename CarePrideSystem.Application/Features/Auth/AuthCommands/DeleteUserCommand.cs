using MediatR;

namespace CarePrideSystem.Application.Features.Auth.AuthCommands
{
    public class DeleteUserCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}