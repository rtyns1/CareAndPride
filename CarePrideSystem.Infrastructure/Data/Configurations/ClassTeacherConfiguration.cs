using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CarePrideSystem.Domain.Entities;

namespace CarePrideSystem.Infrastructure.Data.Configurations
{
    public class ClassTeacherConfiguration : IEntityTypeConfiguration<ClassTeacher>
    {
        public void Configure(EntityTypeBuilder<ClassTeacher> builder)
        {
            builder.ToTable("ClassTeachers");
            builder.HasKey(ct => new { ct.TeacherId, ct.ClassId, ct.AcademicYearId });

            // FK: ClassTeacher -> User (as Teacher)
            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(ct => ct.TeacherId)
                   .OnDelete(DeleteBehavior.Restrict);

            // FK: ClassTeacher -> Class
            builder.HasOne<Class>()
                   .WithMany()
                   .HasForeignKey(ct => ct.ClassId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(ct => ct.TeacherId);
            builder.HasIndex(ct => ct.ClassId);
        }
    }
}
