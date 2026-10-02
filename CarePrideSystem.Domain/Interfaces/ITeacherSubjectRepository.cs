using CarePrideSystem.Domain.Entities;

namespace CarePrideSystem.Domain.Interfaces
{
    public interface ITeacherSubjectRepository
    {
        Task<IEnumerable<TeacherSubject>> GetByTeacherIdAsync(Guid teacherId);
        Task<IEnumerable<TeacherSubject>> GetBySubjectIdAsync(Guid subjectId);
        Task<IEnumerable<TeacherSubject>> GetByClassIdAsync(Guid classId);
        Task<IEnumerable<TeacherSubject>> GetAllAsync();
        Task AddAsync(TeacherSubject entity);
        Task DeleteAsync(Guid teacherId, Guid subjectId, Guid classId, Guid academicYearId);
        Task<bool> ExistsAsync(Guid teacherId, Guid subjectId, Guid classId, Guid academicYearId);
    }
}
