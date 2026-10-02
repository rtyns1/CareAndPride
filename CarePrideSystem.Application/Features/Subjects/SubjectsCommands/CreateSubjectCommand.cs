using MediatR;

namespace CarePrideSystem.Application.Features.Subjects.SubjectsCommands
{
    public class CreateSubjectCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
