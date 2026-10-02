using CarePrideSystem.Domain.Entities;

namespace CarePrideSystem.Domain.Interfaces
{
    public interface ISubjectRepository
    {
        Task<Subject> GetByIdAsync(Guid id);
        Task<Subject> GetByCodeAsync(string code);
        Task<IEnumerable<Subject>> GetAllAsync();
        Task<IEnumerable<Subject>> GetActiveAsync();
        Task AddAsync(Subject entity);
        Task UpdateAsync(Subject entity);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> CodeExistsAsync(string code);
    }
}
