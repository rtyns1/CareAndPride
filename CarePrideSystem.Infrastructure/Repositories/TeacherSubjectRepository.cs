using Microsoft.EntityFrameworkCore;
using CarePrideSystem.Domain.Entities;
using CarePrideSystem.Domain.Interfaces;
using CarePrideSystem.Infrastructure.Data;

namespace CarePrideSystem.Infrastructure.Repositories
{
    public class TeacherSubjectRepository : ITeacherSubjectRepository
    {
        private readonly AppDbContext _context;
        public TeacherSubjectRepository(AppDbContext context) { _context = context; }

        public async Task<IEnumerable<TeacherSubject>> GetByTeacherIdAsync(Guid teacherId) =>
            await _context.TeacherSubjects.Where(ts => ts.TeacherId == teacherId).ToListAsync();

        public async Task<IEnumerable<TeacherSubject>> GetBySubjectIdAsync(Guid subjectId) =>
            await _context.TeacherSubjects.Where(ts => ts.SubjectId == subjectId).ToListAsync();

        public async Task<IEnumerable<TeacherSubject>> GetByClassIdAsync(Guid classId) =>
            await _context.TeacherSubjects.Where(ts => ts.ClassId == classId).ToListAsync();

        public async Task<IEnumerable<TeacherSubject>> GetAllAsync() =>
            await _context.TeacherSubjects.ToListAsync();

        public async Task AddAsync(TeacherSubject entity)
        {
            await _context.TeacherSubjects.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid teacherId, Guid subjectId, Guid classId, Guid academicYearId)
        {
            var entity = await _context.TeacherSubjects.FirstOrDefaultAsync(ts =>
                ts.TeacherId == teacherId && ts.SubjectId == subjectId &&
                ts.ClassId == classId && ts.AcademicYearId == academicYearId);
            if (entity != null) { _context.TeacherSubjects.Remove(entity); await _context.SaveChangesAsync(); }
        }

        public async Task<bool> ExistsAsync(Guid teacherId, Guid subjectId, Guid classId, Guid academicYearId) =>
            await _context.TeacherSubjects.AnyAsync(ts =>
                ts.TeacherId == teacherId && ts.SubjectId == subjectId &&
                ts.ClassId == classId && ts.AcademicYearId == academicYearId);
    }
}
