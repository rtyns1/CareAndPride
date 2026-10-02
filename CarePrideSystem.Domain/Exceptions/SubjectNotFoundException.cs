using CarePrideSystem.Domain.Exceptions;

namespace CarePrideSystem.Domain.Exceptions
{
    public class SubjectNotFoundException : DomainException
    {
        public SubjectNotFoundException(Guid subjectId)
            : base($"Subject with ID {subjectId} was not found.") { }

        public SubjectNotFoundException(string code)
            : base($"Subject with code '{code}' was not found.") { }
    }
}