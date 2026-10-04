using MediatR;
using CarePrideSystem.Application.DTOs.Teachers;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Teachers.Queries
{
    public class GetTeacherSubjectAssignmentsQueryHandler
        : IRequestHandler<GetTeacherSubjectAssignmentsQuery, IEnumerable<TeacherSubjectDto>>
    {
        private readonly ITeacherSubjectRepository _repo;
        public GetTeacherSubjectAssignmentsQueryHandler(ITeacherSubjectRepository repo) { _repo = repo; }

        public async Task<IEnumerable<TeacherSubjectDto>> Handle(
            GetTeacherSubjectAssignmentsQuery request, CancellationToken ct)
        {
            var list = await _repo.GetByTeacherIdAsync(request.TeacherId);
            return list.Select(e => new TeacherSubjectDto
            {
                TeacherId = e.TeacherId,
                SubjectId = e.SubjectId,
                ClassId = e.ClassId,
                AcademicYearId = e.AcademicYearId,
                AssignedAtUtc = e.AssignedAtUtc
            });
        }
    }
}
