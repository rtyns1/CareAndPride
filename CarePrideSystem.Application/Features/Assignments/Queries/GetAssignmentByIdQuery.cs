using MediatR;
using CarePrideSystem.Application.DTOs.Assignments;

namespace CarePrideSystem.Application.Features.Assignments.Queries
{
    public class GetAssignmentByIdQuery : IRequest<AssignmentDto?>
    {
        public Guid Id { get; set; }
    }
}
