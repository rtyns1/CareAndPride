using CarePrideSystem.Domain.Exceptions;
using CarePrideSystem.Domain.Interfaces;
using CarePrideSystem.Domain.Interfaces;
using MediatR;

namespace CarePrideSystem.Application.Features.Auth.AuthCommands
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id);

            if (user == null)
                throw new UserNotFoundException(request.Id);

            var existingUserByEmail = await _userRepository.GetByEmailAsync(request.Email);
            if (existingUserByEmail != null && existingUserByEmail.Id != request.Id)
                throw new InvalidOperationException($"Email '{request.Email}' is already in use.");

            user.UpdateDetails(request.FullName, request.Email);

            await _userRepository.UpdateAsync(user);

            return true;
        }
    }
}
