using MediatR;
using CarePrideSystem.Domain.Exceptions;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Subjects.SubjectsCommands
{
    public class DeleteSubjectCommandHandler : IRequestHandler<DeleteSubjectCommand, bool>
    {
        private readonly ISubjectRepository _repo;
        public DeleteSubjectCommandHandler(ISubjectRepository repo) { _repo = repo; }

        public async Task<bool> Handle(DeleteSubjectCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetByIdAsync(request.Id);
            if (entity == null) throw new SubjectNotFoundException(request.Id);

            entity.Deactivate();
            await _repo.UpdateAsync(entity);
            return true;
        }
    }
}
