using CarePrideSystem.Domain.Exceptions;

namespace CarePrideSystem.Domain.Exceptions
{
    public class StudentAlreadyEnrolledException : DomainException
    {
        public StudentAlreadyEnrolledException(Guid studentId, Guid classId)
            : base($"Student {studentId} is already enrolled in class {classId}.") { }

        public StudentAlreadyEnrolledException(string admissionNumber, string className)
            : base($"Student with admission number '{admissionNumber}' is already enrolled in {className}.") { }
    }
}