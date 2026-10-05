using Microsoft.EntityFrameworkCore;
using CarePrideSystem.Domain.Entities;
using CarePrideSystem.Domain.Interfaces;
using CarePrideSystem.Infrastructure.Data;

namespace CarePrideSystem.Infrastructure.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly AppDbContext _context;
        public AssignmentRepository(AppDbContext context) { _context = context; }

        public async Task<Assignment> GetByIdAsync(Guid id) =>
            await _context.Assignments.FirstOrDefaultAsync(a => a.Id == id);

        public async Task<IEnumerable<Assignment>> GetByClassIdAsync(Guid classId) =>
            await _context.Assignments.Where(a => a.ClassId == classId).ToListAsync();

        public async Task<IEnumerable<Assignment>> GetBySubjectIdAsync(Guid subjectId) =>
            await _context.Assignments.Where(a => a.SubjectId == subjectId).ToListAsync();

        public async Task<IEnumerable<Assignment>> GetByTeacherIdAsync(Guid teacherId) =>
            await _context.Assignments.Where(a => a.TeacherId == teacherId).ToListAsync();

        public async Task<IEnumerable<Assignment>> GetByStudentClassAsync(Guid studentId) =>
            await _context.Assignments.ToListAsync();

        public async Task<IEnumerable<Assignment>> GetAllAsync() =>
            await _context.Assignments.ToListAsync();

        public async Task AddAsync(Assignment assignment)
        {
            await _context.Assignments.AddAsync(assignment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Assignment assignment)
        {
            _context.Assignments.Update(assignment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var a = await GetByIdAsync(id);
            if (a != null) { _context.Assignments.Remove(a); await _context.SaveChangesAsync(); }
        }

        public async Task<bool> ExistsAsync(Guid id) =>
            await _context.Assignments.AnyAsync(a => a.Id == id);
    }
}
