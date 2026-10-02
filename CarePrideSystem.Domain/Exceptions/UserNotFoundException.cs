using CarePrideSystem.Domain.Exceptions;

namespace CarePrideSystem.Domain.Exceptions
{
    public class UserNotFoundException : DomainException
    {
        public UserNotFoundException(Guid userId)
            : base($"User with ID {userId} was not found.") { }

        public UserNotFoundException(string username)
            : base($"User with username '{username}' was not found.") { }
    }
}