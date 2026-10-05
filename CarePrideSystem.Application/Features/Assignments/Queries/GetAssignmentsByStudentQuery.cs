using MediatR;
using CarePrideSystem.Application.DTOs.Assignments;

namespace CarePrideSystem.Application.Features.Assignments.Queries
{
    public class GetAssignmentsByStudentQuery : IRequest<IEnumerable<AssignmentDto>>
    {
        public Guid StudentId { get; set; }
    }
}
