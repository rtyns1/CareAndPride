using MediatR;

namespace CarePrideSystem.Application.Features.Subjects.SubjectsCommands
{
    public class UpdateSubjectCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
