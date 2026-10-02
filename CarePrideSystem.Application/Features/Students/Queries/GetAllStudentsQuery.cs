using MediatR;
using CarePrideSystem.Application.DTOs.Students;

namespace CarePrideSystem.Application.Features.Students.Queries
{
    public class GetAllStudentsQuery : IRequest<IEnumerable<StudentDto>>
    {
    }
}
