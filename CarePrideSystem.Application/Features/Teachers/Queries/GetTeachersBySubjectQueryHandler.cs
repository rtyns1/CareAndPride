using MediatR;
using CarePrideSystem.Application.DTOs.Teachers;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Teachers.Queries
{
    public class GetTeachersBySubjectQueryHandler : IRequestHandler<GetTeachersBySubjectQuery, IEnumerable<TeacherSubjectDto>>
    {
        private readonly ITeacherSubjectRepository _repo;
        public GetTeachersBySubjectQueryHandler(ITeacherSubjectRepository repo) { _repo = repo; }

        public async Task<IEnumerable<TeacherSubjectDto>> Handle(GetTeachersBySubjectQuery request, CancellationToken cancellationToken)
        {
            var list = await _repo.GetBySubjectIdAsync(request.SubjectId);
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
