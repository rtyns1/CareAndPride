using MediatR;
using CarePrideSystem.Application.DTOs.Students;

namespace CarePrideSystem.Application.Features.Students.Queries
{
    public class GetStudentByClassQuery : IRequest<IEnumerable<StudentDto>>
    {
        public Guid ClassId { get; set; }
    }
}
