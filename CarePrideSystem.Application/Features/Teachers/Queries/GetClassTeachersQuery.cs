using MediatR;
using CarePrideSystem.Application.DTOs.Teachers;

namespace CarePrideSystem.Application.Features.Teachers.Queries
{
    public class GetClassTeachersQuery : IRequest<IEnumerable<ClassTeacherDto>>
    {
        public Guid ClassId { get; set; }
    }
}
