using CarePrideSystem.Domain.Entities;

namespace CarePrideSystem.Domain.Interfaces
{
    public interface IAssignmentRepository
    {
        Task<Assignment> GetByIdAsync(Guid id);
        Task<IEnumerable<Assignment>> GetByClassIdAsync(Guid classId);
        Task<IEnumerable<Assignment>> GetBySubjectIdAsync(Guid subjectId);
        Task<IEnumerable<Assignment>> GetByTeacherIdAsync(Guid teacherId);
        Task<IEnumerable<Assignment>> GetAllAsync();
        Task AddAsync(Assignment a);
    }
}
