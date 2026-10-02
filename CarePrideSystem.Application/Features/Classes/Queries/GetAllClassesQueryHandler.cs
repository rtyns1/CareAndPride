using MediatR;
using CarePrideSystem.Application.DTOs.Classes;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Classes.Queries
{
    public class GetAllClassesQueryHandler : IRequestHandler<GetAllClassesQuery, IEnumerable<ClassDto>>
    {
        private readonly IClassRepository _repo;
        public GetAllClassesQueryHandler(IClassRepository repo) { _repo = repo; }

        public async Task<IEnumerable<ClassDto>> Handle(GetAllClassesQuery request, CancellationToken cancellationToken)
        {
            var list = await _repo.GetAllAsync();
            return list.Select(e => new ClassDto
            {
                Id = e.Id,
                Name = e.Name,
                GradeLevel = e.GradeLevel,
                Capacity = e.Capacity,
                AcademicYearId = e.AcademicYearId,
                IsActive = e.IsActive,
                CreatedAtUtc = e.CreatedAtUtc
            });
        }
    }
}
