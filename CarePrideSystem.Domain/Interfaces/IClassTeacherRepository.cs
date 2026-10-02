using CarePrideSystem.Domain.Entities;

namespace CarePrideSystem.Domain.Interfaces
{
    public interface IClassTeacherRepository
    {
        Task<IEnumerable<ClassTeacher>> GetByTeacherIdAsync(Guid teacherId);
        Task<IEnumerable<ClassTeacher>> GetByClassIdAsync(Guid classId);
        Task<IEnumerable<ClassTeacher>> GetAllAsync();
        Task<ClassTeacher> GetPrimaryClassTeacherAsync(Guid classId, Guid academicYearId);
        Task AddAsync(ClassTeacher entity);
        Task UpdateAsync(ClassTeacher entity);
        Task DeleteAsync(Guid teacherId, Guid classId, Guid academicYearId);
        Task<bool> ExistsAsync(Guid teacherId, Guid classId, Guid academicYearId);
        Task<bool> HasPrimaryClassTeacherAsync(Guid classId, Guid academicYearId);
    }
}
