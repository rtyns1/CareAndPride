using MediatR;
using CarePrideSystem.Application.DTOs.Subjects;
using CarePrideSystem.Domain.Exceptions;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Subjects.Queries
{
    public class GetSubjectByIdQueryHandler : IRequestHandler<GetSubjectByIdQuery, SubjectDto>
    {
        private readonly ISubjectRepository _repo;
        public GetSubjectByIdQueryHandler(ISubjectRepository repo) { _repo = repo; }

        public async Task<SubjectDto> Handle(GetSubjectByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetByIdAsync(request.Id);
            if (entity == null) throw new SubjectNotFoundException(request.Id);

            return new SubjectDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Code = entity.Code,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedAtUtc = entity.CreatedAtUtc
            };
        }
    }
}
