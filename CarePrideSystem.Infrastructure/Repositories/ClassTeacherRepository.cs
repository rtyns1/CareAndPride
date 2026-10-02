using Microsoft.EntityFrameworkCore;
using CarePrideSystem.Domain.Entities;
using CarePrideSystem.Domain.Interfaces;
using CarePrideSystem.Infrastructure.Data;

namespace CarePrideSystem.Infrastructure.Repositories
{
    public class ClassTeacherRepository : IClassTeacherRepository
    {
        private readonly AppDbContext _context;
        public ClassTeacherRepository(AppDbContext context) { _context = context; }

        public async Task<IEnumerable<ClassTeacher>> GetByTeacherIdAsync(Guid teacherId) =>
            await _context.ClassTeachers.Where(ct => ct.TeacherId == teacherId).ToListAsync();

        public async Task<IEnumerable<ClassTeacher>> GetByClassIdAsync(Guid classId) =>
            await _context.ClassTeachers.Where(ct => ct.ClassId == classId).ToListAsync();

        public async Task<IEnumerable<ClassTeacher>> GetAllAsync() =>
            await _context.ClassTeachers.ToListAsync();

        public async Task<ClassTeacher> GetPrimaryClassTeacherAsync(Guid classId, Guid academicYearId) =>
            await _context.ClassTeachers.FirstOrDefaultAsync(ct =>
                ct.ClassId == classId && ct.AcademicYearId == academicYearId && ct.IsPrimary);

        public async Task AddAsync(ClassTeacher entity)
        {
            await _context.ClassTeachers.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ClassTeacher entity)
        {
            _context.ClassTeachers.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid teacherId, Guid classId, Guid academicYearId)
        {
            var entity = await _context.ClassTeachers.FirstOrDefaultAsync(ct =>
                ct.TeacherId == teacherId && ct.ClassId == classId && ct.AcademicYearId == academicYearId);
            if (entity != null) { _context.ClassTeachers.Remove(entity); await _context.SaveChangesAsync(); }
        }

        public async Task<bool> ExistsAsync(Guid teacherId, Guid classId, Guid academicYearId) =>
            await _context.ClassTeachers.AnyAsync(ct =>
                ct.TeacherId == teacherId && ct.ClassId == classId && ct.AcademicYearId == academicYearId);

        public async Task<bool> HasPrimaryClassTeacherAsync(Guid classId, Guid academicYearId) =>
            await _context.ClassTeachers.AnyAsync(ct =>
                ct.ClassId == classId && ct.AcademicYearId == academicYearId && ct.IsPrimary);
    }
}
