using Microsoft.EntityFrameworkCore;
using CarePrideSystem.Domain.Entities;
using CarePrideSystem.Domain.Interfaces;
using CarePrideSystem.Infrastructure.Data;

namespace CarePrideSystem.Infrastructure.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly AppDbContext _c;
        public AssignmentRepository(AppDbContext c) { _c = c; }

        public Task<Assignment> GetByIdAsync(Guid id) => _c.Assignments.FirstOrDefaultAsync(a => a.Id == id);
        public async Task<IEnumerable<Assignment>> GetByClassIdAsync(Guid classId) =>
            await _c.Assignments.Where(a => a.ClassId == classId).ToListAsync();
        public async Task<IEnumerable<Assignment>> GetBySubjectIdAsync(Guid subjectId) =>
            await _c.Assignments.Where(a => a.SubjectId == subjectId).ToListAsync();
        public async Task<IEnumerable<Assignment>> GetByTeacherIdAsync(Guid teacherId) =>
            await _c.Assignments.Where(a => a.TeacherId == teacherId).ToListAsync();
        public async Task<IEnumerable<Assignment>> GetAllAsync() => await _c.Assignments.ToListAsync();
        public async Task AddAsync(Assignment a) { await _c.Assignments.AddAsync(a); await _c.SaveChangesAsync(); }
    }
}
