using MediatR;

namespace CarePrideSystem.Application.Features.Students.StudentsCommands
{
    public class ArchiveStudentCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
