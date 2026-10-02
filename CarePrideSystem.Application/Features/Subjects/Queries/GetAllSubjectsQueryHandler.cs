using MediatR;
using CarePrideSystem.Application.DTOs.Subjects;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Subjects.Queries
{
    public class GetAllSubjectsQueryHandler : IRequestHandler<GetAllSubjectsQuery, IEnumerable<SubjectDto>>
    {
        private readonly ISubjectRepository _repo;
        public GetAllSubjectsQueryHandler(ISubjectRepository repo) { _repo = repo; }

        public async Task<IEnumerable<SubjectDto>> Handle(GetAllSubjectsQuery request, CancellationToken cancellationToken)
        {
            var list = await _repo.GetAllAsync();
            return list.Select(e => new SubjectDto
            {
                Id = e.Id,
                Name = e.Name,
                Code = e.Code,
                Description = e.Description,
                IsActive = e.IsActive,
                CreatedAtUtc = e.CreatedAtUtc
            });
        }
    }
}
