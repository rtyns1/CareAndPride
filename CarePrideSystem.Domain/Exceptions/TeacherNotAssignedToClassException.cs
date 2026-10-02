using CarePrideSystem.Domain.Exceptions;

namespace CarePrideSystem.Domain.Exceptions
{
    public class TeacherNotAssignedToClassException : DomainException
    {
        public TeacherNotAssignedToClassException(Guid teacherId, Guid classId)
            : base($"Teacher {teacherId} is not assigned to class {classId}.") { }

        public TeacherNotAssignedToClassException(string teacherName, string className)
            : base($"Teacher '{teacherName}' is not assigned to class '{className}'.") { }
    }
}