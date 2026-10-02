using MediatR;

namespace CarePrideSystem.Application.Features.Classes.ClassesCommands
{
    public class CreateClassCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public int GradeLevel { get; set; }
        public int Capacity { get; set; }
        public Guid AcademicYearId { get; set; }
    }
}
