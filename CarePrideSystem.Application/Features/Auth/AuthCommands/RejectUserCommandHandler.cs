using CarePrideSystem.Domain.Exceptions;
using CarePrideSystem.Domain.Interfaces;
using CarePrideSystem.Domain.Interfaces;
using MediatR;

namespace CarePrideSystem.Application.Features.Auth.AuthCommands
{
    public class RejectUserCommandHandler : IRequestHandler<RejectUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public RejectUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(RejectUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user == null)
                throw new UserNotFoundException(request.Id);

            user.Reject();

            await _userRepository.UpdateAsync(user);

            return true;
        }
    }
}
