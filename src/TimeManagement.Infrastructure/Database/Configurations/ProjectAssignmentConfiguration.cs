using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TimeManagement.Domain.Entities;

namespace TimeManagement.Infrastructure.Database.Configurations;

public class ProjectAssignmentConfiguration : IEntityTypeConfiguration<ProjectAssignment>
{
    public void Configure(EntityTypeBuilder<ProjectAssignment> builder)
    {
        builder.HasKey(pa => new { pa.EmployeeId, pa.ProjectId });

        builder.HasOne(pa => pa.Employee)
               .WithMany(e => e.ProjectAssignments)
               .HasForeignKey(pa => pa.EmployeeId);

        builder.HasOne(pa => pa.Project)
               .WithMany(p => p.ProjectAssignments)
               .HasForeignKey(pa => pa.ProjectId);
    }
}
