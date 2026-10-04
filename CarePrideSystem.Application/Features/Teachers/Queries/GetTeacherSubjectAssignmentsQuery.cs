using MediatR;
using CarePrideSystem.Application.DTOs.Teachers;

namespace CarePrideSystem.Application.Features.Teachers.Queries
{
    public class GetTeacherSubjectAssignmentsQuery : IRequest<IEnumerable<TeacherSubjectDto>>
    {
        public Guid TeacherId { get; set; }
    }
}
