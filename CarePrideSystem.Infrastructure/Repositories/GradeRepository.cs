using Microsoft.EntityFrameworkCore;
using CarePrideSystem.Domain.Entities;
using CarePrideSystem.Domain.Interfaces;
using CarePrideSystem.Infrastructure.Data;

namespace CarePrideSystem.Infrastructure.Repositories
{
    public class GradeRepository : IGradeRepository
    {
        private readonly AppDbContext _context;
        public GradeRepository(AppDbContext context) { _context = context; }

        public async Task<Grade> GetByIdAsync(Guid id) =>
            await _context.Grades.FirstOrDefaultAsync(g => g.Id == id);

        public async Task<IEnumerable<Grade>> GetByStudentIdAsync(Guid studentId) =>
            await _context.Grades.Where(g => g.StudentId == studentId).ToListAsync();

        public async Task<IEnumerable<Grade>> GetByClassIdAsync(Guid classId) =>
            await _context.Grades.Where(g => g.ClassId == classId).ToListAsync();

        public async Task<IEnumerable<Grade>> GetByStudentAndSubjectAsync(Guid studentId, Guid subjectId) =>
            await _context.Grades.Where(g => g.StudentId == studentId && g.SubjectId == subjectId).ToListAsync();

        public async Task<IEnumerable<Grade>> GetAllAsync() =>
            await _context.Grades.ToListAsync();

        public async Task AddAsync(Grade grade)
        {
            await _context.Grades.AddAsync(grade);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Grade grade)
        {
            _context.Grades.Update(grade);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var g = await GetByIdAsync(id);
            if (g != null) { _context.Grades.Remove(g); await _context.SaveChangesAsync(); }
        }

        public async Task<bool> ExistsAsync(Guid id) =>
            await _context.Grades.AnyAsync(g => g.Id == id);
    }
}
