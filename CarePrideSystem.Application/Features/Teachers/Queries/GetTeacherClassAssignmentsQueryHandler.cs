using MediatR;
using CarePrideSystem.Application.DTOs.Teachers;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Teachers.Queries
{
    public class GetTeacherClassAssignmentsQueryHandler
        : IRequestHandler<GetTeacherClassAssignmentsQuery, IEnumerable<ClassTeacherDto>>
    {
        private readonly IClassTeacherRepository _repo;
        public GetTeacherClassAssignmentsQueryHandler(IClassTeacherRepository repo) { _repo = repo; }

        public async Task<IEnumerable<ClassTeacherDto>> Handle(
            GetTeacherClassAssignmentsQuery request, CancellationToken ct)
        {
            var list = await _repo.GetByTeacherIdAsync(request.TeacherId);
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
