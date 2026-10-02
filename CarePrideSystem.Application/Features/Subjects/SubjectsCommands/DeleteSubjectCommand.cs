using MediatR;

namespace CarePrideSystem.Application.Features.Subjects.SubjectsCommands
{
    public class DeleteSubjectCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
