using MediatR;
using CarePrideSystem.Application.DTOs.Grades;

namespace CarePrideSystem.Application.Features.Grades.Queries
{
    public class GetGradesByStudentQuery : IRequest<IEnumerable<GradeDto>>
    {
        public Guid StudentId { get; set; }
    }
}
