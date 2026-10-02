using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CarePrideSystem.Domain.Entities;

namespace CarePrideSystem.Infrastructure.Data.Configurations
{
    public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> b)
        {
            b.ToTable("Assignments");
            b.HasKey(a => a.Id);
            b.Property(a => a.Title).IsRequired().HasMaxLength(200);
            b.Property(a => a.Description).HasMaxLength(2000);
            b.Property(a => a.MaxScore).HasPrecision(10, 2);
        }
    }
}
