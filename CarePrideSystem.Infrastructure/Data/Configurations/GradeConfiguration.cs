using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CarePrideSystem.Domain.Entities;

namespace CarePrideSystem.Infrastructure.Data.Configurations
{
    public class GradeConfiguration : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.ToTable("Grades");
            builder.HasKey(g => g.Id);
            builder.Property(g => g.ExamType).IsRequired().HasMaxLength(50);
            builder.Property(g => g.Term).IsRequired().HasMaxLength(50);
            builder.Property(g => g.Score).HasColumnType("decimal(10,2)");
            builder.Property(g => g.MaxScore).HasColumnType("decimal(10,2)");
            builder.Property(g => g.GradeLetter).IsRequired().HasMaxLength(5);
            builder.Property(g => g.Remarks).HasMaxLength(500);
            builder.HasIndex(g => new { g.StudentId, g.SubjectId, g.ExamType, g.Term });
            builder.HasIndex(g => g.ClassId);
            builder.HasIndex(g => g.RecordedByTeacherId);
        }
    }
}
