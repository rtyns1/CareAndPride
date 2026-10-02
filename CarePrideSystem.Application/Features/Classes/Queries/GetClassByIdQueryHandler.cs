using MediatR;
using CarePrideSystem.Application.DTOs.Classes;
using CarePrideSystem.Domain.Exceptions;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Classes.Queries
{
    public class GetClassByIdQueryHandler : IRequestHandler<GetClassByIdQuery, ClassDto>
    {
        private readonly IClassRepository _repo;
        public GetClassByIdQueryHandler(IClassRepository repo) { _repo = repo; }

        public async Task<ClassDto> Handle(GetClassByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetByIdAsync(request.Id);
            if (entity == null) throw new ClassNotFoundException(request.Id);

            return new ClassDto
            {
                Id = entity.Id,
                Name = entity.Name,
                GradeLevel = entity.GradeLevel,
                Capacity = entity.Capacity,
                AcademicYearId = entity.AcademicYearId,
                IsActive = entity.IsActive,
                CreatedAtUtc = entity.CreatedAtUtc
            };
        }
    }
}
