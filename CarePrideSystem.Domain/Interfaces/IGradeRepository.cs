using CarePrideSystem.Domain.Entities;

namespace CarePrideSystem.Domain.Interfaces
{
    public interface IGradeRepository
    {
        Task<Grade> GetByIdAsync(Guid id);
        Task<IEnumerable<Grade>> GetByStudentIdAsync(Guid studentId);
        Task<IEnumerable<Grade>> GetByClassIdAsync(Guid classId);
        Task<IEnumerable<Grade>> GetByStudentAndSubjectAsync(Guid studentId, Guid subjectId);
        Task<IEnumerable<Grade>> GetAllAsync();
        Task AddAsync(Grade grade);
        Task UpdateAsync(Grade grade);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
