using MediatR;
using CarePrideSystem.Application.DTOs.Students;

namespace CarePrideSystem.Application.Features.Students.Queries
{
    public class GetStudentDetailQuery : IRequest<StudentDetailDto?>
    {
        public Guid StudentId { get; set; }
    }
}
