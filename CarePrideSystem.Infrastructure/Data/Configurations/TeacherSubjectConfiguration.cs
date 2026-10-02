using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CarePrideSystem.Domain.Entities;

namespace CarePrideSystem.Infrastructure.Data.Configurations
{
    public class TeacherSubjectConfiguration : IEntityTypeConfiguration<TeacherSubject>
    {
        public void Configure(EntityTypeBuilder<TeacherSubject> builder)
        {
            builder.ToTable("TeacherSubjects");
            builder.HasKey(ts => new { ts.TeacherId, ts.SubjectId, ts.ClassId, ts.AcademicYearId });

            // FK: TeacherSubject -> User (as Teacher)
            builder.HasOne<User>()
                   .WithMany()
                   .HasForeignKey(ts => ts.TeacherId)
                   .OnDelete(DeleteBehavior.Restrict);

            // FK: TeacherSubject -> Subject
            builder.HasOne<Subject>()
                   .WithMany()
                   .HasForeignKey(ts => ts.SubjectId)
                   .OnDelete(DeleteBehavior.Restrict);

            // FK: TeacherSubject -> Class
            builder.HasOne<Class>()
                   .WithMany()
                   .HasForeignKey(ts => ts.ClassId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(ts => ts.TeacherId);
            builder.HasIndex(ts => ts.SubjectId);
            builder.HasIndex(ts => ts.ClassId);
        }
    }
}
