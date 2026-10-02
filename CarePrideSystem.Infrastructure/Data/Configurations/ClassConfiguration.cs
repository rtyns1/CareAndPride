using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CarePrideSystem.Domain.Entities;

namespace CarePrideSystem.Infrastructure.Data.Configurations
{
    public class ClassConfiguration : IEntityTypeConfiguration<Class>
    {
        public void Configure(EntityTypeBuilder<Class> builder)
        {
            builder.ToTable("Classes");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name).IsRequired().HasMaxLength(50);
            builder.Property(c => c.GradeLevel).IsRequired();
            builder.Property(c => c.Capacity).IsRequired();

            builder.HasIndex(c => c.Name).IsUnique();
            builder.HasIndex(c => c.AcademicYearId);
        }
    }
}
