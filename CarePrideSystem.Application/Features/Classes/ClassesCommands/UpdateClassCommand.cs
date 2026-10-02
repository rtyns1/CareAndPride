using MediatR;

namespace CarePrideSystem.Application.Features.Classes.ClassesCommands
{
    public class UpdateClassCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int GradeLevel { get; set; }
        public int Capacity { get; set; }
    }
}
