using CarePrideSystem.Domain.Entities;

namespace CarePrideSystem.Domain.Interfaces
{
    public interface ISubmissionRepository
    {
        Task<IEnumerable<Submission>> GetByAssignmentIdAsync(Guid assignmentId);
        Task<IEnumerable<Submission>> GetByStudentIdAsync(Guid studentId);
        Task AddAsync(Submission s);
    }
}
