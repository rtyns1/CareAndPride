using MediatR;
using CarePrideSystem.Application.DTOs.Subjects;

namespace CarePrideSystem.Application.Features.Subjects.Queries
{
    public class GetAllSubjectsQuery : IRequest<IEnumerable<SubjectDto>>
    {
    }
}
