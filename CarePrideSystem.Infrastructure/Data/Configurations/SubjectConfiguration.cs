using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CarePrideSystem.Domain.Entities;

namespace CarePrideSystem.Infrastructure.Data.Configurations
{
    public class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.ToTable("Subjects");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Name).IsRequired().HasMaxLength(100);
            builder.Property(s => s.Code).IsRequired().HasMaxLength(20);
            builder.Property(s => s.Description).HasMaxLength(500);
            builder.HasIndex(s => s.Code).IsUnique();
        }
    }
}
