using MediatR;

namespace CarePrideSystem.Application.Features.Classes.ClassesCommands
{
    public class DeleteClassCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
