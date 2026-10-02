using MediatR;

namespace CarePrideSystem.Application.Features.Teachers.TeachersCommands
{
    public class AssignTeacherSubjectCommand : IRequest<bool>
    {
        public Guid TeacherId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid ClassId { get; set; }
        public Guid AcademicYearId { get; set; }
    }
}
