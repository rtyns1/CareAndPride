using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CarePrideSystem.Domain.Entities;

namespace CarePrideSystem.Infrastructure.Data.Configurations
{
    public class SubmissionConfiguration : IEntityTypeConfiguration<Submission>
    {
        public void Configure(EntityTypeBuilder<Submission> b)
        {
            b.ToTable("Submissions");
            b.HasKey(s => s.Id);
            b.Property(s => s.Feedback).HasMaxLength(1000);
            b.Property(s => s.Score).HasPrecision(10, 2);
            b.HasIndex(s => s.AssignmentId);
            b.HasIndex(s => s.StudentId);
        }
    }
}
