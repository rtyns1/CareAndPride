using MediatR;
using CarePrideSystem.Application.DTOs.Teachers;

namespace CarePrideSystem.Application.Features.Teachers.Queries
{
    public class GetTeacherClassAssignmentsQuery : IRequest<IEnumerable<ClassTeacherDto>>
    {
        public Guid TeacherId { get; set; }
    }
}
