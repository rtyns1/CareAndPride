using MediatR;
using CarePrideSystem.Domain.Exceptions;
using CarePrideSystem.Domain.Interfaces;

namespace CarePrideSystem.Application.Features.Classes.ClassesCommands
{
    public class DeleteClassCommandHandler : IRequestHandler<DeleteClassCommand, bool>
    {
        private readonly IClassRepository _classRepository;

        public DeleteClassCommandHandler(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public async Task<bool> Handle(DeleteClassCommand request, CancellationToken cancellationToken)
        {
            var entity = await _classRepository.GetByIdAsync(request.Id);
            if (entity == null) throw new ClassNotFoundException(request.Id);

            entity.Deactivate();
            await _classRepository.UpdateAsync(entity);
            return true;
        }
    }
}
