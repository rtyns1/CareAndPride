using MediatR;
using CarePrideSystem.Application.DTOs.Teachers;

namespace CarePrideSystem.Application.Features.Teachers.Queries
{
    public class GetTeachersBySubjectQuery : IRequest<IEnumerable<TeacherSubjectDto>>
    {
        public Guid SubjectId { get; set; }
    }
}
