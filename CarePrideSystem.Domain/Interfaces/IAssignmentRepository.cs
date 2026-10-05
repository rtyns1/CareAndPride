using CarePrideSystem.Domain.Entities;

namespace CarePrideSystem.Domain.Interfaces
{
    public interface IAssignmentRepository
    {
        Task<Assignment> GetByIdAsync(Guid id);
        Task<IEnumerable<Assignment>> GetByClassIdAsync(Guid classId);
        Task<IEnumerable<Assignment>> GetBySubjectIdAsync(Guid subjectId);
        Task<IEnumerable<Assignment>> GetByTeacherIdAsync(Guid teacherId);
        Task<IEnumerable<Assignment>> GetByStudentClassAsync(Guid studentId);
        Task<IEnumerable<Assignment>> GetAllAsync();
        Task AddAsync(Assignment assignment);
        Task UpdateAsync(Assignment assignment);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
