using CarePrideSystem.Domain.Exceptions;

namespace CarePrideSystem.Domain.Exceptions
{
    public class ClassNotFoundException : DomainException
    {
        public ClassNotFoundException(Guid classId)
            : base($"Class with ID {classId} was not found.") { }

        public ClassNotFoundException(string className)
            : base($"Class '{className}' was not found.") { }
    }
}