using CarePrideSystem.Domain.Entities;

namespace CarePrideSystem.Domain.Interfaces
{
    public interface IClassRepository
    {
        Task<Class> GetByIdAsync(Guid id);
        Task<IEnumerable<Class>> GetAllAsync();
        Task<IEnumerable<Class>> GetActiveAsync();
        Task AddAsync(Class entity);
        Task UpdateAsync(Class entity);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
        Task<bool> NameExistsAsync(string name);
    }
}
