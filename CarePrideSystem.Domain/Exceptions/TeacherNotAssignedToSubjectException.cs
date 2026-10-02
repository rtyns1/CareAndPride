using CarePrideSystem.Domain.Exceptions;

namespace CarePrideSystem.Domain.Exceptions
{
    public class TeacherNotAssignedToSubjectException : DomainException
    {
        public TeacherNotAssignedToSubjectException(Guid teacherId, Guid subjectId)
            : base($"Teacher {teacherId} is not assigned to subject {subjectId}.") { }

        public TeacherNotAssignedToSubjectException(string teacherName, string subjectName)
            : base($"Teacher '{teacherName}' is not assigned to subject '{subjectName}'.") { }
    }
}