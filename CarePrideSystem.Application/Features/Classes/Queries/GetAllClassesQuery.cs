using MediatR;
using CarePrideSystem.Application.DTOs.Classes;

namespace CarePrideSystem.Application.Features.Classes.Queries
{
    public class GetAllClassesQuery : IRequest<IEnumerable<ClassDto>>
    {
    }
}
