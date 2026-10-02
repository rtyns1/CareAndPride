using CarePrideSystem.Domain.Exceptions;
using CarePrideSystem.Domain.Interfaces;
using CarePrideSystem.Domain.Interfaces;
using MediatR;

namespace CarePrideSystem.Application.Features.Auth.AuthCommands
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user == null)
                throw new UserNotFoundException(request.Id);

            await _userRepository.DeleteAsync(request.Id);

            return true;
        }
    }
}
