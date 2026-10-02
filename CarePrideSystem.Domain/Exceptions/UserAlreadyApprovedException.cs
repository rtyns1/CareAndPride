using CarePrideSystem.Domain.Exceptions;
using System;


namespace CarePrideSystem.Domain.Exceptions
{
    public class UserAlreadyApprovedException : DomainException
    {
        public UserAlreadyApprovedException(Guid userId)
            : base($"User {userId} is already approved.") { }
    }
}