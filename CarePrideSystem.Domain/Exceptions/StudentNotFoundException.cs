using CarePrideSystem.Domain.Exceptions;

namespace CarePrideSystem.Domain.Exceptions
{
    public class StudentNotFoundException : DomainException
    {
        public StudentNotFoundException(Guid studentId)
            : base($"Student with ID {studentId} was not found.") { }

        public StudentNotFoundException(string admissionNumber)
            : base($"Student with admission number '{admissionNumber}' was not found.") { }
    }
}