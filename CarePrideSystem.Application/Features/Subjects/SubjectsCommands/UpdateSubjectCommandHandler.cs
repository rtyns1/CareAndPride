using MediatR;
using CarePrideSystem.Domain.Exceptions;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Subjects.SubjectsCommands
{
    public class UpdateSubjectCommandHandler : IRequestHandler<UpdateSubjectCommand, bool>
    {
        private readonly ISubjectRepository _repo;
        public UpdateSubjectCommandHandler(ISubjectRepository repo) { _repo = repo; }

        public async Task<bool> Handle(UpdateSubjectCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repo.GetByIdAsync(request.Id);
            if (entity == null) throw new SubjectNotFoundException(request.Id);

            entity.UpdateDetails(request.Name, request.Code, request.Description);
            await _repo.UpdateAsync(entity);
            return true;
        }
    }
}
