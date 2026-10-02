using MediatR;
using CarePrideSystem.Application.DTOs.Classes;

namespace CarePrideSystem.Application.Features.Classes.Queries
{
    public class GetClassByIdQuery : IRequest<ClassDto>
    {
        public Guid Id { get; set; }
    }
}
