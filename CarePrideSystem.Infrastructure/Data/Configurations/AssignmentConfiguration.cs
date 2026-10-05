using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CarePrideSystem.Domain.Entities;

namespace CarePrideSystem.Infrastructure.Data.Configurations
{
    public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.ToTable("Assignments");
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Title).IsRequired().HasMaxLength(200);
            builder.Property(a => a.Description).HasMaxLength(2000);
            builder.Property(a => a.ExamType).IsRequired().HasMaxLength(50);
            builder.Property(a => a.MaxScore).HasColumnType("decimal(10,2)");
            builder.Property(a => a.FilePath).HasMaxLength(500);
            builder.Property(a => a.OriginalFileName).HasMaxLength(255);
            builder.HasIndex(a => a.ClassId);
            builder.HasIndex(a => a.SubjectId);
            builder.HasIndex(a => a.TeacherId);
        }
    }
}
