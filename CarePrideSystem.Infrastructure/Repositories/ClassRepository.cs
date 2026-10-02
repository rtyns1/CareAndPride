using Microsoft.EntityFrameworkCore;
using CarePrideSystem.Domain.Entities;
using CarePrideSystem.Domain.Interfaces;
using CarePrideSystem.Infrastructure.Data;

namespace CarePrideSystem.Infrastructure.Repositories
{
    public class ClassRepository : IClassRepository
    {
        private readonly AppDbContext _context;
        public ClassRepository(AppDbContext context) { _context = context; }

        public async Task<Class> GetByIdAsync(Guid id) => await _context.Classes.FirstOrDefaultAsync(c => c.Id == id);
        public async Task<IEnumerable<Class>> GetAllAsync() => await _context.Classes.ToListAsync();
        public async Task<IEnumerable<Class>> GetActiveAsync() => await _context.Classes.Where(c => c.IsActive).ToListAsync();

        public async Task AddAsync(Class entity)
        {
            await _context.Classes.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Class entity)
        {
            _context.Classes.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null) { _context.Classes.Remove(entity); await _context.SaveChangesAsync(); }
        }

        public async Task<bool> ExistsAsync(Guid id) => await _context.Classes.AnyAsync(c => c.Id == id);
        public async Task<bool> NameExistsAsync(string name) => await _context.Classes.AnyAsync(c => c.Name == name);
    }
}
