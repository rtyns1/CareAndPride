using MediatR;
using CarePrideSystem.Application.DTOs.Subjects;

namespace CarePrideSystem.Application.Features.Subjects.Queries
{
    public class GetSubjectByIdQuery : IRequest<SubjectDto>
    {
        public Guid Id { get; set; }
    }
}
