using MediatR;
using CarePrideSystem.Application.DTOs.Teachers;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Teachers.Queries
{
    public class GetClassTeachersQueryHandler : IRequestHandler<GetClassTeachersQuery, IEnumerable<ClassTeacherDto>>
    {
        private readonly IClassTeacherRepository _repo;
        public GetClassTeachersQueryHandler(IClassTeacherRepository repo) { _repo = repo; }

        public async Task<IEnumerable<ClassTeacherDto>> Handle(GetClassTeachersQuery request, CancellationToken cancellationToken)
        {
            var list = await _repo.GetByClassIdAsync(request.ClassId);
            return list.Select(e => new ClassTeacherDto
            {
                TeacherId = e.TeacherId,
                ClassId = e.ClassId,
                AcademicYearId = e.AcademicYearId,
                IsPrimary = e.IsPrimary,
                AssignedAtUtc = e.AssignedAtUtc
            });
        }
    }
}
