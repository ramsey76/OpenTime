using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeManagement.Domain.Entities;

namespace TimeManagement.Infrastructure.Database.Configurations;

public class TimeEntryConfiguration : IEntityTypeConfiguration<TimeEntry>
{
    public void Configure(EntityTypeBuilder<TimeEntry> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Hours).HasPrecision(4, 2);
        builder.Property(t => t.Description).HasMaxLength(500);

        builder.HasOne(t => t.Employee)
               .WithMany()
               .HasForeignKey(t => t.EmployeeId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.Project)
               .WithMany(p => p.TimeEntries)
               .HasForeignKey(t => t.ProjectId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
