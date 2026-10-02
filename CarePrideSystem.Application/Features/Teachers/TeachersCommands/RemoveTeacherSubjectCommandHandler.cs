using MediatR;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Teachers.TeachersCommands
{
    public class RemoveTeacherSubjectCommandHandler : IRequestHandler<RemoveTeacherSubjectCommand, bool>
    {
        private readonly ITeacherSubjectRepository _repo;
        public RemoveTeacherSubjectCommandHandler(ITeacherSubjectRepository repo) { _repo = repo; }

        public async Task<bool> Handle(RemoveTeacherSubjectCommand request, CancellationToken cancellationToken)
        {
            if (!await _repo.ExistsAsync(request.TeacherId, request.SubjectId, request.ClassId, request.AcademicYearId))
                throw new InvalidOperationException("This assignment does not exist.");

            await _repo.DeleteAsync(request.TeacherId, request.SubjectId, request.ClassId, request.AcademicYearId);
            return true;
        }
    }
}
