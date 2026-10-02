using MediatR;
using CarePrideSystem.Application.DTOs.Students;

namespace CarePrideSystem.Application.Features.Students.Queries
{
    public class GetStudentByIdQuery : IRequest<StudentDto>
    {
        public Guid Id { get; set; }
    }
}
