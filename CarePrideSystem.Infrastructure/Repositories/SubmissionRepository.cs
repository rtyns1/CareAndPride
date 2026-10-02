using Microsoft.EntityFrameworkCore;
using CarePrideSystem.Domain.Entities;
using CarePrideSystem.Domain.Interfaces;
using CarePrideSystem.Infrastructure.Data;

namespace CarePrideSystem.Infrastructure.Repositories
{
    public class SubmissionRepository : ISubmissionRepository
    {
        private readonly AppDbContext _c;
        public SubmissionRepository(AppDbContext c) { _c = c; }

        public async Task<IEnumerable<Submission>> GetByAssignmentIdAsync(Guid assignmentId) =>
            await _c.Submissions.Where(s => s.AssignmentId == assignmentId).ToListAsync();
        public async Task<IEnumerable<Submission>> GetByStudentIdAsync(Guid studentId) =>
            await _c.Submissions.Where(s => s.StudentId == studentId).ToListAsync();
        public async Task AddAsync(Submission s) { await _c.Submissions.AddAsync(s); await _c.SaveChangesAsync(); }
    }
}
