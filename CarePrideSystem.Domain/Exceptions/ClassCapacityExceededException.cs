using CarePrideSystem.Domain.Exceptions;

namespace CarePrideSystem.Domain.Exceptions
{
    public class ClassCapacityExceededException : DomainException
    {
        public ClassCapacityExceededException(Guid classId, int capacity, int currentEnrollment)
            : base($"Class {classId} has reached maximum capacity of {capacity}. Current enrollment: {currentEnrollment}.") { }

        public ClassCapacityExceededException(string className, int capacity, int currentEnrollment)
            : base($"Class '{className}' has reached maximum capacity of {capacity}. Current enrollment: {currentEnrollment}.") { }
    }
}