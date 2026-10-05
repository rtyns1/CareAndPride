using MediatR;
using CarePrideSystem.Application.DTOs.Assignments;

namespace CarePrideSystem.Application.Features.Assignments.Queries
{
    public class GetAssignmentsByClassQuery : IRequest<IEnumerable<AssignmentDto>>
    {
        public Guid ClassId { get; set; }
    }
}
