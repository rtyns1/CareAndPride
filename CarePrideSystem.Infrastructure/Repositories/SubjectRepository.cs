using Microsoft.EntityFrameworkCore;
using CarePrideSystem.Domain.Entities;
using CarePrideSystem.Domain.Interfaces;
using CarePrideSystem.Infrastructure.Data;

namespace CarePrideSystem.Infrastructure.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly AppDbContext _context;
        public SubjectRepository(AppDbContext context) { _context = context; }

        public async Task<Subject> GetByIdAsync(Guid id) => await _context.Subjects.FirstOrDefaultAsync(s => s.Id == id);
        public async Task<Subject> GetByCodeAsync(string code) => await _context.Subjects.FirstOrDefaultAsync(s => s.Code == code);
        public async Task<IEnumerable<Subject>> GetAllAsync() => await _context.Subjects.ToListAsync();
        public async Task<IEnumerable<Subject>> GetActiveAsync() => await _context.Subjects.Where(s => s.IsActive).ToListAsync();

        public async Task AddAsync(Subject entity)
        {
            await _context.Subjects.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Subject entity)
        {
            _context.Subjects.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null) { _context.Subjects.Remove(entity); await _context.SaveChangesAsync(); }
        }

        public async Task<bool> ExistsAsync(Guid id) => await _context.Subjects.AnyAsync(s => s.Id == id);
        public async Task<bool> CodeExistsAsync(string code) => await _context.Subjects.AnyAsync(s => s.Code == code);
    }
}
