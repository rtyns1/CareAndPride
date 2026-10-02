using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CarePrideSystem.Domain.Entities;

namespace CarePrideSystem.Infrastructure.Data.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable("Students");
            builder.HasKey(s => s.Id);

            builder.Property(s => s.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(s => s.LastName).IsRequired().HasMaxLength(50);
            builder.Property(s => s.AdmissionNumber).IsRequired().HasMaxLength(50);
            builder.Property(s => s.MedicalConditions).HasMaxLength(500);

            builder.HasIndex(s => s.AdmissionNumber).IsUnique();

            // FK: Student -> Class (Restrict: cannot delete a class with students)
            builder.HasOne<Class>()
                   .WithMany()
                   .HasForeignKey(s => s.ClassId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(s => s.ClassId);
        }
    }
}
